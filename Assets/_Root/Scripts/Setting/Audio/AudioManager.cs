using Gamee_Hiukka.Data;
using Gamee_Hiukka.Pattern;
using System.Collections;
using UnityEngine;
using static Gamee_Hiukka.Data.AudioResource;

namespace Gamee_Hiukka.Control 
{
    public class AudioManager : Singleton<AudioManager>
    {
        public AudioResource audioResoures;
        public AudioSource audioSoure;
        public AudioSource audioSoureMusic;

        public void PlayAudioButtonClick() 
        {
            PlayAudio(audioResoures.audioButtonClick);
        }
        public void PlayAudioButtonClickStartGame()
        {
            PlayAudio(audioResoures.audioButtonClickStartGame);
        }
        public void PlayAudioCoinMove()
        {
            PlayAudio(audioResoures.audioCoinMove);
        }

        public void PlayAudioGameWin()
        {
            PlayAudio(audioResoures.audioGameWin);
        }

        public void PlayAudioGameLose()
        {
            PlayAudio(audioResoures.audioGameLose);
        }

        // main
        public void PlayAudioPlayerAttack1()
        {
            PlayAudio(audioResoures.audioPlayerAttack1);
        }
        public void PlayAudioPlayerAttack2()
        {
            PlayAudio(audioResoures.audioPlayerAttack2);
        }
        public void PlayAudioPlayerYawn()
        {
            PlayAudio(audioResoures.audioPlayerYawn);
        }
        public void PlayAudioPlayerDone()
        {
            PlayAudio(audioResoures.audioPlayerDone);
        }
        public void PlayAudioPlayerDeath()
        {
            PlayAudio(audioResoures.audioPlayerDeath);
        }

        public void PlayAudioPlayerCry()
        {
            PlayAudio(audioResoures.audioPlayerCry);
        }
        public void PlayAudioPlayerPickKey()
        {
            PlayAudio(audioResoures.audioPlayerPickKey);
        }
        public void PlayAudioPlayerOpenChest()
        {
            PlayAudio(audioResoures.audioPlayerOpenChest);
        }
        public void PlayAudioPlayerSeeTarget()
        {
            PlayAudio(audioResoures.audioPlayerSeeTarget);
        }
        public void PlayAudioPlayerMove()
        {
            PlayAudio(audioResoures.audioPlayerMove);
        }
        // popy
        public void PlayAudioPopyAttack()
        {
            PlayAudio(audioResoures.audioPopyAttack);
        }
        // enemy
        public void PlayAudioEnemyMaleDie()
        {
            PlayAudio(audioResoures.audioEnemyMaleDie);
        }
        public void PlayAudioEnemyFemaleDie()
        {
            PlayAudio(audioResoures.audioEnemyFemaleDie);
        }
        public void PlayAudioEnemyMaleScream()
        {
            PlayAudio(audioResoures.audioEnemyMaleScream);
        }
        public void PlayAudioEnemyFemaleScream()
        {
            PlayAudio(audioResoures.audioEnemyFemaleScream);
        }
        public void PlayAudioEnemyGunFire()
        {
            PlayAudio(audioResoures.audioEnemyGunFire);
        }
        public void PlayAudioEnemyHit()
        {
            PlayAudio(audioResoures.audioEnemyHit);
        }

        // pin 
        public void PlayAudioPinMove()
        {
            PlayAudio(audioResoures.audioPinMove);
        }
        public void PlayAudioPinStuck()
        {
            PlayAudio(audioResoures.audioPinStuck);
        }

        // elements
        public void PlayAudioBombBreak()
        {
            PlayAudio(audioResoures.audioBombBreak);
        }
        public void PlayAudioBombMineBreak()
        {
            PlayAudio(audioResoures.audioBombMineBreak);
        }
        public void PlayAudioTrapUse()
        {
            PlayAudio(audioResoures.audioTrapUse);
        }
        public void PlayAudioTrap()
        {
            PlayAudio(audioResoures.audioTrap);
        }
        public void PlayAudioArrowTrapAttack()
        {
            PlayAudio(audioResoures.audioArrowTrapAttack);
        }
        public void PlayAudioSawUse()
        {
            PlayAudio(audioResoures.audioSawUse);
        }
        public void PlayAudioCutRope()
        {
            PlayAudio(audioResoures.audioCutRope);
        }
        public void PlayAudioOpenCabinet()
        {
            PlayAudio(audioResoures.audioOpenCabinet);
        }
        public void PlayAudioTeleportIn()
        {
            PlayAudio(audioResoures.audioTeleportIn);
        }
        public void PlayAudioTeleportOut()
        {
            PlayAudio(audioResoures.audioTeleportOut);
        }
        public void PlayAudioSwitch()
        {
            PlayAudio(audioResoures.audioSwitch);
        }
        public void PlayAudioBoxFire()
        {
            PlayAudio(audioResoures.audioBoxFire);
        }

        // liquid
        public void PlayAudioLiquidCollison()
        {
            PlayAudio(audioResoures.audioLiquidCollision);
        }

        public void PlayAudioIceCollison()
        {
            PlayAudio(audioResoures.audioIceCollision);
        }
        public void PlayAudioStoneCollison()
        {
            PlayAudio(audioResoures.audioStoneCollision);
        }
        public void PlayAudioStoneLiquidCollison()
        {
            PlayAudio(audioResoures.audioStoneLiquidCollision);
        }
        public void PlayAudioBoxCollison()
        {
            PlayAudio(audioResoures.audioBoxCollision);
        }
        public void PlayAudioWaterFire()
        {
            PlayAudio(audioResoures.audioWaterFire);
        }
        public void PlayAudioWaterIce()
        {
            PlayAudio(audioResoures.audioWaterFire);
        }
        // popup
        public void PlayAudioNewSkin()
        {
            PlayAudio(audioResoures.audioNewSkin);
        }
        public void PlayAudioUnlockSkin()
        {
            PlayAudio(audioResoures.audioUnlockSkin);
        }
        public void PlayAudioUnlockBox()
        {
            PlayAudio(audioResoures.audioUnlockBox);
        }
        public void PlayAudioGetEggpPiece()
        {
            PlayAudio(audioResoures.audioGetEggPiece);
        }
        public void PlayAudioBuilding()
        {
            PlayAudio(audioResoures.audioBuilding);
        }
        public void PlayAudioBuildCompleted()
        {
            PlayAudio(audioResoures.audioBuildCompleted);
        }
        public void PlayAudioRoomBuildCompleted()
        {
            PlayAudio(audioResoures.audioRoomBuildCompleted);
        }
        public void PlayAudioOpenNewRoom()
        {
            PlayAudio(audioResoures.audioOpenNewRoom);
        }
        // back ground
        public void PlayAudioBackGroundGameMenu() 
        {
            PlayAudioBackGround(audioResoures.audioBGGameMenu);
        }
        public void PlayAudioBackGroundGamePlay()
        {
            PlayAudioBackGround(audioResoures.audioBGGamePlay);
        }

        // handle
        private void PlayAudioBackGround(Sound soundBG)
        {
            audioSoureMusic.clip = soundBG.audioClip;
            audioSoureMusic.volume = soundBG.volume;
            audioSoureMusic.loop = true;

            if (GameData.MusicStatus)
            {
                audioSoureMusic.Play();
            }
        }

        public void PlayAudio(Sound sound)
        {
            if (sound == null) return;

            if (GameData.AudioStatus)
            {
                audioSoure.PlayOneShot(sound.audioClip, sound.volume);
            }
        }

        public void PauseAuido() 
        {
            if(audioSoure.isPlaying) audioSoure.Stop();
        }

        public void PlayMusic() 
        {
            if (!audioSoureMusic.isPlaying)
            {
                audioSoureMusic.Play();
            }
        }
        public void PauseMusic() 
        {
            if (audioSoureMusic.isPlaying)
            {
                audioSoureMusic.Pause();
            }
        }
    }
}

