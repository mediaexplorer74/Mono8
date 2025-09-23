
using System;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Input;
using Mono8.Enums;

/*
    Big thanks to https://github.com/MonoGame/MonoGame.Samples/blob/3.8.4/AutoPong/AutoPong.Core/Game/AudioSource.cs
*/

namespace Mono8;
public class Sound8 {

    // Audio 
    private int sampleRate = 48000; 
    private AudioChannels channels = AudioChannels.Mono;

    private byte[] soundBuffer;
    private int soundBufferSize;

    private int totalTime = 0;


    // Classes
    private DynamicSoundEffectInstance dynamicSoundEffectInstance;
    private static Random random = new Random();

    public Sound8() {

        // Set Classes
        dynamicSoundEffectInstance = new DynamicSoundEffectInstance(sampleRate, channels);

        // Int
        soundBufferSize = dynamicSoundEffectInstance.GetSampleSizeInBytes(TimeSpan.FromMilliseconds(500));
        soundBuffer = new byte[soundBufferSize];

        dynamicSoundEffectInstance.Volume = 0.5f;
        dynamicSoundEffectInstance.IsLooped = false;

    }

    /// <summary>
    /// Method <c>PlaySoundWave</c> plays a sound way from a frequency
    /// </summary>
    public void PlaySoundWave(double frequency,  short durationInMS, SoundWaveType soundWaveType, float volume) {
        
        // stop all sounds 
        dynamicSoundEffectInstance.Stop();


        soundBufferSize = dynamicSoundEffectInstance.GetSampleSizeInBytes(TimeSpan.FromMilliseconds(durationInMS));
        soundBuffer = new byte[soundBufferSize];


        int size = soundBufferSize - 1;
        for(int i = 0; i < size; i += 2) {

            double time = (double)totalTime / (double)sampleRate;

            short currentSample = 0;
            switch (soundWaveType) {

                case SoundWaveType.NOISE:
                   currentSample = (short)(random.Next(-short.MaxValue, short.MaxValue) * volume);
                break;

                case SoundWaveType.SIN:
                    currentSample = (short)(Math.Sin(2 * Math.PI * frequency * time) * (double)short.MaxValue * volume);
                break;

                case SoundWaveType.SQUARE:
                    currentSample = (short)(Math.Sign(Math.Sin(2 * Math.PI * frequency * time)) * (double)short.MaxValue * volume);
                break;

                case SoundWaveType.TAN:
                    currentSample = (short)(Math.Tan(2 * Math.PI * frequency * time) * (double)short.MaxValue * volume);
                break;
        
            }

            soundBuffer[i] = (byte)(currentSample & 0xFF);
            soundBuffer[i + 1] = (byte)(currentSample >> 8);

            totalTime += 2; 

        }

        dynamicSoundEffectInstance.SubmitBuffer(soundBuffer);
        dynamicSoundEffectInstance.Play();

    }

}