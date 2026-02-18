namespace IQ_AUDIO_OUT
{
    public class IqToneGenerator(int sampleRate) : NAudio.Wave.IWaveProvider
    {
        private readonly int sampleRate = sampleRate;
        private double frequency = 1000.0;      // Частота первого тона
        private double secondToneFrequency = 1500.0; // Частота второго тона
        private double amplitude = 0.1;
        private float iqBalance = 1.0f;
        private float iqPhase = 0.0f;
        private double dcI = 0.0;
        private double dcQ = 0.0;
        private bool swapIQ = false;
        private bool muteI = false;
        private bool muteQ = false;
        private bool isNoiseMode = false;
        private bool isDualToneMode = false;    // Двухтоновый режим
        private readonly Random random = new();
        private int samplePosition = 0;

        // Калибровка шума: +30 дБ для соответствия уровней
        private const double NOISE_AMPLITUDE_BOOST = 31.622776601683793; // 10^(30/20)

        public NAudio.Wave.WaveFormat WaveFormat { get; } = NAudio.Wave.WaveFormat.CreateIeeeFloatWaveFormat(sampleRate, 2);

        public void SetFrequency(int freq) => frequency = freq;
        public void SetSecondToneFrequency(int freq) => secondToneFrequency = freq;
        public void SetAmplitude(double amp) => amplitude = amp;
        public void SetIqBalance(float balance) => iqBalance = balance;
        public void SetIqPhase(float phase) => iqPhase = phase;
        public void SetDcI(double offset) => dcI = offset;
        public void SetDcQ(double offset) => dcQ = offset;
        public void SetSwapIq(bool swap) => swapIQ = swap;
        public void SetMuteI(bool mute) => muteI = mute;
        public void SetMuteQ(bool mute) => muteQ = mute;
        public void SetNoiseMode(bool enabled) => isNoiseMode = enabled;
        public void SetDualToneMode(bool enabled) => isDualToneMode = enabled;

        public int Read(byte[] buffer, int offset, int count)
        {
            int samples = count / 4;

            for (int n = 0; n < samples / 2; n++)
            {
                double iSample, qSample;

                if (isNoiseMode)
                {
                    // Белый шум с калибровкой +30 дБ
                    double noiseI = (random.NextDouble() * 2.0 - 1.0) * amplitude * NOISE_AMPLITUDE_BOOST;
                    double noiseQ = (random.NextDouble() * 2.0 - 1.0) * amplitude * NOISE_AMPLITUDE_BOOST;
                    iSample = noiseI + dcI;
                    qSample = noiseQ + dcQ;
                }
                else if (isDualToneMode)
                {
                    // === Двухтоновый режим ===
                    // Это обеспечивает корректный уровень каждого тона для тестирования IMD
                    double t = (double)samplePosition / sampleRate;
                    double phaseRad = iqPhase * Math.PI / 180.0;

                    // Первый тон с полной амплитудой
                    double i1 = amplitude * Math.Cos(2 * Math.PI * frequency * t) * iqBalance;
                    double q1 = amplitude * Math.Sin(2 * Math.PI * frequency * t + phaseRad);

                    // Второй тон с полной амплитудой
                    double i2 = amplitude * Math.Cos(2 * Math.PI * secondToneFrequency * t) * iqBalance;
                    double q2 = amplitude * Math.Sin(2 * Math.PI * secondToneFrequency * t + phaseRad);

                    iSample = i1 + i2 + dcI;
                    qSample = q1 + q2 + dcQ;
                }
                else
                {
                    // Одиночный тон
                    double t = (double)samplePosition / sampleRate;
                    double phaseRad = iqPhase * Math.PI / 180.0;
                    iSample = amplitude * Math.Cos(2 * Math.PI * frequency * t) * iqBalance + dcI;
                    qSample = amplitude * Math.Sin(2 * Math.PI * frequency * t + phaseRad) + dcQ;
                }

                if (swapIQ) (iSample, qSample) = (qSample, iSample);
                if (muteI) iSample = 0.0;
                if (muteQ) qSample = 0.0;

                BitConverter.GetBytes((float)iSample).CopyTo(buffer, offset + n * 8);
                BitConverter.GetBytes((float)qSample).CopyTo(buffer, offset + n * 8 + 4);

                samplePosition++;
            }

            return count;
        }
    }
}