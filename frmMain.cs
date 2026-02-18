using NAudio.CoreAudioApi;
using NAudio.Wave;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;

[assembly: AssemblyCopyright("© R9OFG 2026")]

namespace IQ_AUDIO_OUT
{
    public partial class FrmMain : Form
    {
        #region Объявления класса
        // Аудио-выходной поток и генератор тона
        private WasapiOut? audioOutput;
        private IqToneGenerator? toneGenerator;

        // Текущее выбранное аудиоустройство вывода и его параметры
        private MMDevice? currentOutputDevice;
        private WaveFormat? deviceWaveFormat;
        private int deviceBitsPerSample;

        private int lastBaseFrequency = 1000; // Последнее значение основной частоты ДО изменения
        #endregion

        #region Конструктор
        public FrmMain()
        {
            InitializeComponent();

            // Настройка поведения окна: фиксированный размер, запрет максимизации, ручное позиционирование
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = true;
            this.StartPosition = FormStartPosition.Manual;

            // Инициализация меток параметров устройства (значения по умолчанию при остановке генерации)
            LblSampleRate.Text = "Sample Rate:";
            LblBitDepth.Text = "Bit Depth:";

            // Настройка трекбара частоты (диапазон ±20 кГц, шаг 100 Гц, центр 1 кГц)
            TbrFrequency.Minimum = -20000;
            TbrFrequency.Maximum = 20000;
            TbrFrequency.Value = 1000;
            lastBaseFrequency = TbrFrequency.Value;
            TbrFrequency.SmallChange = 100;
            TbrFrequency.LargeChange = 1000;
            TbrFrequency.TickFrequency = 5000;
            LblFrequency.Text = $"Frequency: {TbrFrequency.Value} Hz";

            // Настройка трекбара второго тона (диапазон ±5000 Гц от основного тона)
            TbrSecondTone.Minimum = -25000;  // Будет динамически изменяться через UpdateSecondToneRange()
            TbrSecondTone.Maximum = 25000;   // Будет динамически изменяться через UpdateSecondToneRange()
            TbrSecondTone.Value = 1000;
            TbrSecondTone.SmallChange = 10;  // Шаг при нажатии стрелок клавиатуры
            TbrSecondTone.LargeChange = 100; // Шаг при клике на трекбар или Page Up/Down
            TbrSecondTone.TickFrequency = 500;
            TbrSecondTone.Enabled = false;   // Начальное состояние — заблокирован
            LblSecondTone.Text = "2nd Tone: —";

            // Настройка трекбара уровня сигнала (диапазон -130..0 дБ)
            TbrLevel.Minimum = -130;
            TbrLevel.Maximum = 0;
            TbrLevel.Value = -73;
            TbrFrequency.SmallChange = 1;
            TbrLevel.LargeChange = 10;
            TbrLevel.TickFrequency = 10;
            LblLevel.Text = $"Level: {TbrLevel.Value} dB";

            // Настройка трекбара баланса I/Q (50%..150%)
            TbrIQBalance.Minimum = 50;
            TbrIQBalance.Maximum = 150;
            TbrIQBalance.Value = 100;
            TbrIQBalance.SmallChange = 1;
            TbrIQBalance.LargeChange = 10;
            TbrIQBalance.TickFrequency = 10;
            LblIQBalance.Text = $"IQ Balance: {TbrIQBalance.Value / 100.0:F2}";

            // Настройка трекбара фазы I/Q (±45°)
            TbrIQPhase.Minimum = -450;
            TbrIQPhase.Maximum = 450;
            TbrIQPhase.Value = 0;
            TbrIQPhase.SmallChange = 1;
            TbrIQPhase.LargeChange = 10;
            TbrIQPhase.TickFrequency = 50;
            LblIQPhase.Text = $"IQ Phase: {TbrIQPhase.Value / 10.0:F1}°";

            // Настройка трекбаров DC-смещения (диапазон ±250 мВ с шагом 1 мВ)
            TbrDcI.Minimum = -250;
            TbrDcI.Maximum = 250;
            TbrDcI.Value = 0;
            TbrDcI.SmallChange = 1;
            TbrDcI.LargeChange = 10;
            TbrDcI.TickFrequency = 50;
            LblDcI.Text = FormatDcLabel("I", TbrDcI.Value);

            TbrDcQ.Minimum = -250;
            TbrDcQ.Maximum = 250;
            TbrDcQ.Value = 0;
            TbrDcQ.SmallChange = 1;
            TbrDcQ.LargeChange = 10;
            TbrDcQ.TickFrequency = 50;
            LblDcQ.Text = FormatDcLabel("Q", TbrDcQ.Value);

            // Обработчик кнопки настроек устройства
            BtnDeviceSettings.Click += BtnDeviceSettings_Click;

            // Загрузка списка доступных аудиоустройств вывода
            LoadOutputDevices();

            // Загрузка сохранённых настроек формы из INI-файла
            var settings = new SettingsManager();
            settings.LoadForm(this);

            // Защита от некорректных сохранённых значений DC-смещения (ограничение диапазона ±250 мВ)
            TbrDcI.Value = Math.Clamp(TbrDcI.Value, TbrDcI.Minimum, TbrDcI.Maximum);
            TbrDcQ.Value = Math.Clamp(TbrDcQ.Value, TbrDcQ.Minimum, TbrDcQ.Maximum);

            // Инициализация меток
            UpdateAllLabels();
            UpdateDeviceFormatLabels();

            // Обработчики событий прокрутки трекбаров
            TbrFrequency.Scroll += TbrFrequency_Scroll;
            TbrSecondTone.Scroll += TbrSecondTone_Scroll;
            TbrLevel.Scroll += TbrLevel_Scroll;
            TbrIQBalance.Scroll += TbrIQBalance_Scroll;
            TbrIQPhase.Scroll += TbrIQPhase_Scroll;
            TbrDcI.Scroll += TbrDcI_Scroll;
            TbrDcQ.Scroll += TbrDcQ_Scroll;

            // Обработчики чекбоксов
            ChkDualTone.CheckedChanged += ChkDualTone_CheckedChanged;
            ChkSwapIQ.CheckedChanged += ChkSwapIQ_CheckedChanged;
            ChkNoise.CheckedChanged += ChkNoise_CheckedChanged;
            ChkMuteI.CheckedChanged += ChkMuteI_CheckedChanged;
            ChkMuteQ.CheckedChanged += ChkMuteQ_CheckedChanged;

            // Обработчик клика по ссылке автора
            LnkR9OFG.LinkClicked += LnkR9OFG_LinkClicked;

            // Настройка сброса параметров по двойному клику на метке
            // Двойной клик по метке основной частоты: сброс основного тона + синхронный сброс второго тона
            LblFrequency.DoubleClick += (_, _) =>
            {
                const int defaultFreq = 1000; // Значение по умолчанию для основного тона

                // 1. Сброс основного тона
                TbrFrequency.Value = defaultFreq;
                toneGenerator?.SetFrequency(defaultFreq);

                // 2. Сброс второго тона в двухтоновом режиме
                if (ChkDualTone.Checked)
                {
                    // Обновляем границы второго тона под новую основную частоту
                    int minSecond = defaultFreq - 5000;
                    int maxSecond = defaultFreq + 5000;

                    TbrSecondTone.Minimum = minSecond;
                    TbrSecondTone.Maximum = maxSecond;

                    // Устанавливаем второй тон на +1000 Гц от основного (значение по умолчанию)
                    int defaultSecond = defaultFreq + 1000;
                    defaultSecond = (int)Math.Round(defaultSecond / 10.0) * 10; // Выравнивание до 10 Гц
                    defaultSecond = Math.Clamp(defaultSecond, minSecond, maxSecond);

                    TbrSecondTone.Value = defaultSecond;
                    toneGenerator?.SetSecondToneFrequency(defaultSecond);

                    // Обновляем последнее значение основной частоты для корректного расчёта дельты
                    lastBaseFrequency = defaultFreq;
                }

                // 3. Обновление всех меток
                UpdateAllLabels();
            };
            LblSecondTone.DoubleClick += (_, _) =>
            {
                if (!ChkDualTone.Checked) return;

                // Сброс второго тона на +500 Гц от основного тона
                int baseFreq = TbrFrequency.Value;
                int defaultSecond = baseFreq + 1000;

                // Обновляем границы
                UpdateSecondToneRange();

                // Выравнивание до шага 10 Гц
                defaultSecond = (int)Math.Round(defaultSecond / 10.0) * 10;
                defaultSecond = Math.Clamp(defaultSecond, TbrSecondTone.Minimum, TbrSecondTone.Maximum);

                TbrSecondTone.Value = defaultSecond;
                LblSecondTone.Text = $"2nd Tone: {defaultSecond} Hz";
                toneGenerator?.SetSecondToneFrequency(defaultSecond);

                // Обновляем дельту для последующих перемещений
                lastBaseFrequency = baseFreq;
            };
            BindReset(LblLevel, TbrLevel, -73, v => toneGenerator?.SetAmplitude((v <= -130) ? 0.0 : Math.Pow(10.0, v / 20.0)));
            BindReset(LblIQBalance, TbrIQBalance, 100, v => toneGenerator?.SetIqBalance(v / 100.0f));
            BindReset(LblIQPhase, TbrIQPhase, 0, v => toneGenerator?.SetIqPhase(v / 10.0f));
            BindReset(LblDcI, TbrDcI, 0, v => toneGenerator?.SetDcI(v / 1000.0f));
            BindReset(LblDcQ, TbrDcQ, 0, v => toneGenerator?.SetDcQ(v / 1000.0f));

            

            // Обработчикиы выбора устройства и обновления списка при раскрытии
            CmbOutputDevices.SelectedIndexChanged += CmbOutputDevices_SelectedIndexChanged;
            CmbOutputDevices.DropDown += (s, e) => LoadOutputDevices();

            // Настройка контекстного меню S-метра (предустановки уровней)
            s1ToolStripMenuItem.Click += (_, _) => SetLevelDb(-121);
            s2ToolStripMenuItem.Click += (_, _) => SetLevelDb(-115);
            s3ToolStripMenuItem.Click += (_, _) => SetLevelDb(-109);
            s4ToolStripMenuItem.Click += (_, _) => SetLevelDb(-103);
            s5ToolStripMenuItem.Click += (_, _) => SetLevelDb(-97);
            s6ToolStripMenuItem.Click += (_, _) => SetLevelDb(-91);
            s7ToolStripMenuItem.Click += (_, _) => SetLevelDb(-85);
            s8ToolStripMenuItem.Click += (_, _) => SetLevelDb(-79);
            s9ToolStripMenuItem.Click += (_, _) => SetLevelDb(-73);
            s9Plus10ToolStripMenuItem.Click += (_, _) => SetLevelDb(-63);
            s9Plus20ToolStripMenuItem.Click += (_, _) => SetLevelDb(-53);
            s9Plus30ToolStripMenuItem.Click += (_, _) => SetLevelDb(-43);
            s9Plus40ToolStripMenuItem.Click += (_, _) => SetLevelDb(-33);

            // Инициализация выбора устройства по умолчанию
            CmbOutputDevices_SelectedIndexChanged(null, EventArgs.Empty);
            ChkDualTone_CheckedChanged(null, EventArgs.Empty); // Установка начального состояния (заблокировать трекбар)
            UpdateButtonStates();
            this.Tag = settings; // Сохранение ссылки на менеджер настроек
        }
        #endregion
                
        #region Вспомогательные методы
        // Форматирование значения DC-смещения для отображения в метке
        // Для |значения| >= 100 мВ: отображение в вольтах (например, +0,100V)
        // Для |значения| < 100 мВ: отображение в милливольтах (например, +0,50mV)
        private static string FormatDcLabel(string prefix, int millivolts)
        {
            if (Math.Abs(millivolts) >= 100)
            {
                double volts = millivolts / 1000.0;
                return $"{prefix} DC Offs: {volts:+0.000;-0.000}V".Replace('.', ',');
            }
            else
            {
                string sign = millivolts >= 0 ? "+" : "-";
                int absMv = Math.Abs(millivolts);
                return $"{prefix} DC Offs: {sign}0,{absMv:D2}mV";
            }
        }

        // Обновление текста всех меток параметров в соответствии с текущими значениями трекбаров
        private void UpdateAllLabels()
        {
            LblFrequency.Text = ChkNoise.Checked
                ? "Frequency: Noise"
                : $"Frequency: {TbrFrequency.Value} Hz";

            LblLevel.Text = $"Level: {TbrLevel.Value} dB";
            LblIQBalance.Text = $"IQ Balance: {TbrIQBalance.Value / 100.0:F2}";
            LblIQPhase.Text = $"IQ Phase: {TbrIQPhase.Value / 10.0:F1}°";
            LblDcI.Text = FormatDcLabel("I", TbrDcI.Value);
            LblDcQ.Text = FormatDcLabel("Q", TbrDcQ.Value);

            // Обновление метки второго тона только если режим активен
            if (ChkDualTone.Checked)
                LblSecondTone.Text = $"2nd Tone: {TbrSecondTone.Value} Hz";
            else
                LblSecondTone.Text = "2nd Tone:";
        }

        // Универсальный метод привязки двойного клика на метку для сброса параметра к значению по умолчанию
        private void BindReset(Label label, TrackBar trackBar, int defaultValue, Action<int> apply)
        {
            label.DoubleClick += (_, _) =>
            {
                trackBar.Value = defaultValue;
                apply(defaultValue);
                UpdateAllLabels(); // Обязательное обновление метки после сброса
            };
        }

        // Установка уровня сигнала через контекстное меню S-метра
        private void SetLevelDb(int dB)
        {
            dB = Math.Clamp(dB, TbrLevel.Minimum, TbrLevel.Maximum);
            TbrLevel.Value = dB;
            LblLevel.Text = $"Level: {dB} dB";
            toneGenerator?.SetAmplitude((dB <= -130) ? 0.0 : Math.Pow(10.0, dB / 20.0));
        }

        // Пересчёт границ трекбара второго тона в зависимости от основной частоты
        private void UpdateSecondToneRange()
        {
            if (!ChkDualTone.Checked) return;

            int baseFreq = TbrFrequency.Value;

            // Диапазон второго тона: основной ±5000 Гц (БЕЗ ограничения ±20000!)
            // Трекбар имеет расширенный диапазон ±25000, поэтому ограничения не нужны
            int minSecond = baseFreq - 5000;
            int maxSecond = baseFreq + 5000;

            // Установка новых границ трекбара
            TbrSecondTone.Minimum = minSecond;
            TbrSecondTone.Maximum = maxSecond;

            // Коррекция текущего значения с выравниванием до шага 10 Гц
            int currentValue = TbrSecondTone.Value;
            currentValue = Math.Clamp(currentValue, minSecond, maxSecond);
            currentValue = (int)Math.Round(currentValue / 10.0) * 10;
            currentValue = Math.Clamp(currentValue, minSecond, maxSecond);

            TbrSecondTone.Value = currentValue;
            LblSecondTone.Text = $"2nd Tone: {currentValue} Hz";
        }
        #endregion

        #region Работа с аудиоустройствами вывода
        // Загрузка списка активных аудиоустройств вывода в комбобокс
        private void LoadOutputDevices()
        {
            CmbOutputDevices.Items.Clear();
            try
            {
                var enumerator = new MMDeviceEnumerator();
                var devices = enumerator.EnumerateAudioEndPoints(DataFlow.Render, DeviceState.Active);
                int index = 0;
                bool hasDevices = false;
                foreach (var device in devices)
                {
                    string name = device.FriendlyName.Trim();
                    if (!string.IsNullOrEmpty(name))
                    {
                        CmbOutputDevices.Items.Add($"[{index}] {name}");
                        index++;
                        hasDevices = true;
                    }
                }

                if (!hasDevices)
                    CmbOutputDevices.Items.Add("No output audio device");
            }
            catch
            {
                CmbOutputDevices.Items.Add("No output audio device");
            }

            // Автовыбор первого устройства при запуске
            if (CmbOutputDevices.SelectedIndex == -1 && CmbOutputDevices.Items.Count > 0)
                CmbOutputDevices.SelectedIndex = 0;

            UpdateDeviceFormatLabels();
        }

        // Обработчик изменения выбранного устройства вывода
        private void CmbOutputDevices_SelectedIndexChanged(object? sender, EventArgs? e)
        {
            currentOutputDevice = null;
            deviceWaveFormat = null;

            if (CmbOutputDevices.SelectedItem is not string item || item == "No output audio device")
                return;

            try
            {
                var enumerator = new MMDeviceEnumerator();
                var devices = enumerator.EnumerateAudioEndPoints(DataFlow.Render, DeviceState.Active);
                int targetIndex = CmbOutputDevices.SelectedIndex;
                if (targetIndex < 0) return;

                int i = 0;
                foreach (var device in devices)
                {
                    if (i == targetIndex)
                    {
                        currentOutputDevice = device;
                        var (sr, bits) = GetDevicePreferredFormat(device);
                        deviceBitsPerSample = bits;
                        deviceWaveFormat = new WaveFormat(sr, bits, 2);
                        break;
                    }
                    i++;
                }
            }
            catch
            {
                currentOutputDevice = null;
                deviceWaveFormat = null;
            }

            UpdateButtonStates();
            UpdateDeviceFormatLabels();
        }

        // Получение предпочтительного формата устройства (частота дискретизации и битность)
        private static (int sampleRate, int bitsPerSample) GetDevicePreferredFormat(MMDevice device)
        {
            var mix = device.AudioClient.MixFormat;

            if (mix is WaveFormatExtensible)
            {
                IntPtr ptr = Marshal.AllocHGlobal(Marshal.SizeOf(mix));
                try
                {
                    Marshal.StructureToPtr(mix, ptr, false);
                    short validBits = Marshal.ReadInt16(ptr, 18); // wValidBitsPerSample
                    return (mix.SampleRate, validBits);
                }
                finally
                {
                    Marshal.FreeHGlobal(ptr);
                }
            }

            return (mix.SampleRate, mix.BitsPerSample);
        }

        // Обновление меток формата устройства (частота дискретизации и битность)
        private void UpdateDeviceFormatLabels()
        {
            if (deviceWaveFormat != null)
            {
                LblSampleRate.Text = $"Sample Rate: {deviceWaveFormat.SampleRate} Hz";
                LblBitDepth.Text = $"Device setting: Shared {deviceBitsPerSample}-bit float";
            }
            else
            {
                LblSampleRate.Text = "Sample Rate:";
                LblBitDepth.Text = "Bit Depth:";
            }
        }
        #endregion

        #region Управление генерацией сигнала
        // Обновление состояния кнопок в зависимости от статуса генерации
        private void UpdateButtonStates()
        {
            bool playing = audioOutput != null;
            bool valid = currentOutputDevice != null && deviceWaveFormat != null && !playing;

            BtnStart.Enabled = valid;
            BtnStop.Enabled = playing;
            CmbOutputDevices.Enabled = !playing; // Блокировка выбора устройства во время генерации
            BtnDeviceSettings.Enabled = !playing;
        }

        // Открытие системных настроек звука Windows
        private void BtnDeviceSettings_Click(object? sender, EventArgs e)
        {
            try { Process.Start("control.exe", "mmsys.cpl,,0"); }
            catch { MessageBox.Show("Cannot open sound settings."); }
        }

        // Обработчики прокрутки трекбаров с немедленным применением параметров к генератору
        private void TbrFrequency_Scroll(object? sender, EventArgs? e)
        {
            const int step = 100;
            int aligned = (int)Math.Round(TbrFrequency.Value / (double)step) * step;
            aligned = Math.Clamp(aligned, TbrFrequency.Minimum, TbrFrequency.Maximum);

            // Для двухтонового режима: синхронное перемещение второго тона
            if (ChkDualTone.Checked && aligned != lastBaseFrequency)
            {
                // Вычисляем дельту от ПОСЛЕДНЕГО значения основного тона
                int delta = TbrSecondTone.Value - lastBaseFrequency;

                // Новые границы второго тона: основной ±5000 Гц
                int minSecond = aligned - 5000;
                int maxSecond = aligned + 5000;

                // Новое значение второго тона с сохранением дельты
                int newSecond = aligned + delta;
                newSecond = Math.Clamp(newSecond, minSecond, maxSecond);
                newSecond = (int)Math.Round(newSecond / 10.0) * 10;
                newSecond = Math.Clamp(newSecond, minSecond, maxSecond);

                // Устанавливаем границы и значение второго тона БЕЗ промежуточных состояний
                TbrSecondTone.Minimum = minSecond;
                TbrSecondTone.Maximum = maxSecond;
                TbrSecondTone.Value = newSecond;

                LblSecondTone.Text = $"2nd Tone: {newSecond} Hz";
                toneGenerator?.SetSecondToneFrequency(newSecond);
            }

            // Обновляем основной тон
            TbrFrequency.Value = aligned;
            LblFrequency.Text = $"Frequency: {aligned} Hz";
            toneGenerator?.SetFrequency(aligned);

            // Сохраняем текущее значение как последнее
            lastBaseFrequency = aligned;
        }

        private void TbrSecondTone_Scroll(object? sender, EventArgs? e)
        {
            const int step = 10;
            int aligned = (int)Math.Round(TbrSecondTone.Value / (double)step) * step;
            aligned = Math.Clamp(aligned, TbrSecondTone.Minimum, TbrSecondTone.Maximum);

            if (TbrSecondTone.Value != aligned)
                TbrSecondTone.Value = aligned;

            LblSecondTone.Text = $"2nd Tone: {aligned} Hz";
            toneGenerator?.SetSecondToneFrequency(aligned);

            // При ручной настройке второго тона обновляем последнее значение основной частоты
            // для корректного расчёта дельты при следующем изменении основного тона
            lastBaseFrequency = TbrFrequency.Value;
        }

        private void TbrLevel_Scroll(object? sender, EventArgs? e)
        {
            int dB = TbrLevel.Value;
            LblLevel.Text = $"Level: {dB} dB";
            double amp = (dB <= -130) ? 0.0 : Math.Pow(10.0, dB / 20.0);
            toneGenerator?.SetAmplitude(amp);
        }

        private void TbrIQBalance_Scroll(object? sender, EventArgs? e)
        {
            float f = TbrIQBalance.Value / 100.0f;
            LblIQBalance.Text = $"IQ Balance: {f:F2}";
            toneGenerator?.SetIqBalance(f);
        }

        private void TbrIQPhase_Scroll(object? sender, EventArgs? e)
        {
            float deg = TbrIQPhase.Value / 10.0f;
            LblIQPhase.Text = $"IQ Phase: {deg:F1}°";
            toneGenerator?.SetIqPhase(deg);
        }

        private void TbrDcI_Scroll(object? sender, EventArgs? e)
        {
            int value = TbrDcI.Value;
            LblDcI.Text = FormatDcLabel("I", value);
            toneGenerator?.SetDcI(value / 1000.0f); // Конвертация мВ → В
        }

        private void TbrDcQ_Scroll(object? sender, EventArgs? e)
        {
            int value = TbrDcQ.Value;
            LblDcQ.Text = FormatDcLabel("Q", value);
            toneGenerator?.SetDcQ(value / 1000.0f);
        }

        // Запуск генерации сигнала
        private void BtnStart_Click(object? sender, EventArgs? e)
        {
            if (audioOutput != null || currentOutputDevice == null) return;

            // Перед стартом генерации повторно запрашиваем актуальный формат устройства
            // (на случай изменений в системных настройках звука после выбора устройства)
            if (currentOutputDevice != null)
            {
                try
                {
                    var (sr, bits) = GetDevicePreferredFormat(currentOutputDevice);
                    deviceBitsPerSample = bits;
                    deviceWaveFormat = new WaveFormat(sr, bits, 2);
                    UpdateDeviceFormatLabels(); // Обновляем метки Sample Rate и Bit Depth
                }
                catch
                {
                    deviceWaveFormat = null;
                }
            }

            // Проверка валидности формата после обновления
            if (deviceWaveFormat == null)
            {
                MessageBox.Show("Unable to get device format. Please select another device.", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Создание генератора с актуальной частотой дискретизации
            toneGenerator = new IqToneGenerator(deviceWaveFormat.SampleRate);

            // Установка параметров сигнала
            toneGenerator.SetFrequency(TbrFrequency.Value);
            toneGenerator.SetDualToneMode(ChkDualTone.Checked);
            toneGenerator.SetSecondToneFrequency(TbrSecondTone.Value);
            toneGenerator.SetAmplitude((TbrLevel.Value <= -130) ? 0.0 : Math.Pow(10.0, TbrLevel.Value / 20.0));
            toneGenerator.SetIqBalance(TbrIQBalance.Value / 100.0f);
            toneGenerator.SetIqPhase(TbrIQPhase.Value / 10.0f);
            toneGenerator.SetDcI(TbrDcI.Value / 1000.0f);
            toneGenerator.SetDcQ(TbrDcQ.Value / 1000.0f);
            toneGenerator.SetSwapIq(ChkSwapIQ.Checked);
            toneGenerator.SetMuteI(ChkMuteI.Checked);
            toneGenerator.SetMuteQ(ChkMuteQ.Checked);

            try
            {
                audioOutput = new WasapiOut(currentOutputDevice, AudioClientShareMode.Shared, false, 50);
                audioOutput.Init(toneGenerator);
                audioOutput.Play();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to start audio output:\n{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                audioOutput?.Dispose();
                audioOutput = null;
                toneGenerator = null;
                return;
            }

            // Обновление состояния интерфейса
            UpdateButtonStates();
        }

        // Остановка генерации сигнала
        private void BtnStop_Click(object? sender, EventArgs? e)
        {
            audioOutput?.Stop();
            audioOutput?.Dispose();
            audioOutput = null;
            toneGenerator = null;
            UpdateButtonStates();
        }

        // Обработчики чекбоксов с немедленным применением изменений к генератору
        private void ChkDualTone_CheckedChanged(object? sender, EventArgs? e)
        {
            bool isDualTone = ChkDualTone.Checked;

            // Взаимоисключение с режимом шума
            if (isDualTone && ChkNoise.Checked)
            {
                ChkNoise.Checked = false;
            }

            // Блокировка/разблокировка второго тона
            TbrSecondTone.Enabled = isDualTone;
            LblSecondTone.Enabled = isDualTone;

            // Метка основной частоты показывает ТОЛЬКО основную частоту
            LblFrequency.Text = $"Frequency: {TbrFrequency.Value} Hz";

            if (isDualTone)
            {
                // Инициализация последнего значения для корректного расчёта дельты
                lastBaseFrequency = TbrFrequency.Value;

                // Установка второго тона на +1000 Гц от основного
                UpdateSecondToneRange();

                int defaultSecond = TbrFrequency.Value + 1000;
                defaultSecond = (int)Math.Round(defaultSecond / 10.0) * 10;
                defaultSecond = Math.Clamp(defaultSecond, TbrSecondTone.Minimum, TbrSecondTone.Maximum);

                TbrSecondTone.Value = defaultSecond;
                LblSecondTone.Text = $"2nd Tone: {defaultSecond} Hz";
                toneGenerator?.SetSecondToneFrequency(defaultSecond);
            }
            else
            {
                LblSecondTone.Text = "2nd Tone:";
            }

            // Применение режима к генератору
            toneGenerator?.SetDualToneMode(isDualTone);
        }
        private void ChkSwapIQ_CheckedChanged(object? sender, EventArgs? e) => toneGenerator?.SetSwapIq(ChkSwapIQ.Checked);
        private void ChkMuteI_CheckedChanged(object? sender, EventArgs? e) => toneGenerator?.SetMuteI(ChkMuteI.Checked);
        private void ChkMuteQ_CheckedChanged(object? sender, EventArgs? e) => toneGenerator?.SetMuteQ(ChkMuteQ.Checked);

        // Обработчик переключения режима шума
        private void ChkNoise_CheckedChanged(object? sender, EventArgs? e)
        {
            bool isNoise = ChkNoise.Checked;

            // Взаимоисключение с двухтоновым режимом
            if (isNoise && ChkDualTone.Checked)
            {
                ChkDualTone.Checked = false;
                // После снятия галки автоматически вызовется ChkDualTone_CheckedChanged
            }

            // Блокировка частоты
            TbrFrequency.Enabled = !isNoise;
            LblFrequency.Enabled = !isNoise;

            LblFrequency.Text = isNoise
                ? "Frequency: Noise"
                : $"Frequency: {TbrFrequency.Value} Hz";

            // Блокировка баланса и фазы
            TbrIQBalance.Enabled = !isNoise;
            LblIQBalance.Enabled = !isNoise;
            TbrIQPhase.Enabled = !isNoise;
            LblIQPhase.Enabled = !isNoise;
            ChkSwapIQ.Enabled = !isNoise;
            ChkMuteI.Enabled = !isNoise;
            ChkMuteQ.Enabled = !isNoise;

            // Применение режима к генератору
            toneGenerator?.SetNoiseMode(isNoise);
        }

        // Открытие сайта автора по клику на ссылку
        private void LnkR9OFG_LinkClicked(object? sender, LinkLabelLinkClickedEventArgs e)
        {
            try { Process.Start(new ProcessStartInfo { FileName = "https://r9ofg.ru", UseShellExecute = true }); }
            catch { MessageBox.Show("Cannot open browser."); }
        }
        #endregion

        #region Обработка закрытия формы
        // Корректная остановка генерации и сохранение настроек при закрытии окна
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            BtnStop_Click(null, null); // Гарантированная остановка перед закрытием
            (this.Tag as SettingsManager)?.SaveForm(this);
            base.OnFormClosing(e);
        }
        #endregion
    }
}