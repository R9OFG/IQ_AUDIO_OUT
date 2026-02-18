using System;
using System.IO;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Reflection;
using System.Drawing;

namespace IQ_AUDIO_OUT
{
    public class SettingsManager
    {
        private readonly string iniPath; // Путь к INI-файлу

        public SettingsManager()
        {
            string exePath = Path.GetDirectoryName( Assembly.GetExecutingAssembly().Location ) ?? ".";
            iniPath = Path.Combine(exePath, "settings.ini");
        }

        public void LoadForm(Form form)
        {
            // По умолчанию — центр экрана
            form.StartPosition = FormStartPosition.CenterScreen;

            if (!File.Exists(iniPath))
                return; // INI нет — остаёмся по центру

            var lines = File.ReadAllLines(iniPath);
            var dict = new Dictionary<string, string?>();

            foreach (var line in lines)
            {
                var parts = line.Split('=', 2);
                if (parts.Length == 2)
                    dict[parts[0]] = parts[1];
            }

            // Восстановление позиции окна
            if (dict.TryGetValue("FormLeft", out string? l) &&
                dict.TryGetValue("FormTop", out string? t) &&
                int.TryParse(l, out int left) &&
                int.TryParse(t, out int top))
            {
                var screen = Screen.PrimaryScreen;
                if (screen != null)
                {
                    var workArea = screen.WorkingArea;
                    var pt = new Point(left, top);

                    // Проверка, что окно не за пределами экрана
                    if (workArea.Contains(pt))
                    {
                        form.StartPosition = FormStartPosition.Manual;
                        form.Left = left;
                        form.Top = top;
                    }
                }
            }

            // Восстановление элементов управления
            foreach (Control ctrl in form.Controls)
            {
                switch (ctrl)
                {
                    case TrackBar tb:
                        if (dict.TryGetValue(tb.Name, out string? valStr) &&
                            valStr != null &&
                            int.TryParse(valStr, out int val))
                        {
                            tb.Value = Math.Max(
                                tb.Minimum,
                                Math.Min(tb.Maximum, val)
                            );
                        }
                        break;

                    case ComboBox cb:
                        if (dict.TryGetValue(cb.Name, out string? sel) && sel != null)
                        {
                            int index = cb.Items.IndexOf(sel);
                            if (index >= 0)
                                cb.SelectedIndex = index;
                        }
                        break;
                }
            }
        }

        public void SaveForm(Form form)
        {
            var lines = new List<string>
            {
                // Сохранение позиции окна
                $"FormLeft={form.Left}",
                $"FormTop={form.Top}"
            };

            // Сохранение элементов управления
            foreach (Control ctrl in form.Controls)
            {
                switch (ctrl)
                {
                    case TrackBar tb:
                        lines.Add($"{tb.Name}={tb.Value}");
                        break;

                    case ComboBox cb when cb.SelectedItem != null:
                        lines.Add($"{cb.Name}={cb.SelectedItem}");
                        break;

                    case CheckBox cb:
                        lines.Add($"{cb.Name}={cb.Checked}");
                        break;
                }
            }

            try
            {
                File.WriteAllLines(iniPath, lines);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Failed to save settings:\n{ex.Message}",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
            }
        }
    }
}
