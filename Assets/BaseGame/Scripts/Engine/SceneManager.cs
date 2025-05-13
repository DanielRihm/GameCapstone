using LCPS.SlipForge.Enum;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;

namespace LCPS.SlipForge
{
    public static class SceneManager
    {
        private static readonly SceneEnum[] _alwaysLoadedScenes = { SceneEnum.Main, SceneEnum.Version, SceneEnum.Menus };

        // TODO: should probably get the scene names from a different source
        private static readonly IDictionary<SceneEnum, string> _scenes = new Dictionary<SceneEnum, string>() {
                        { SceneEnum.Main, "SampleScene" },
                        { SceneEnum.Title, "TitleScene" },
                        { SceneEnum.Version, "VersionScene" },
                        { SceneEnum.Hub, "HubScene" },
                        { SceneEnum.HUD, "HUDScene" },
                        { SceneEnum.Menus, "MenusScene" },
                        { SceneEnum.Dungeon, "DungeonScene" },
                        { SceneEnum.Sandbox, "EnemySandboxScene" }
                    };

        // TODO: use state machine to track scene transitions?

        // ---------------- methods ----------------

        public static void Initialize()
        {
            // TODO: load spash screens first
            UnityEngine.SceneManagement.SceneManager.LoadScene(_scenes[SceneEnum.Title], LoadSceneMode.Additive);
            UnityEngine.SceneManagement.SceneManager.LoadScene(_scenes[SceneEnum.Version], LoadSceneMode.Additive);
            UnityEngine.SceneManagement.SceneManager.LoadScene(_scenes[SceneEnum.Menus], LoadSceneMode.Additive);
        }

        public static void BeginDungeon()
        {
            DataTracker.Instance.DungeonLevel = -1;

            // the hub scene should be loaded at this point
            Assert.IsTrue(UnityEngine.SceneManagement.SceneManager.GetSceneByName(_scenes[SceneEnum.Hub]).IsValid());

            // unload the hub scene
            UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(_scenes[SceneEnum.Hub]);

            // Load the Dungeon scene
            DataTracker.Instance.IsDungeon.Value = true;
            SceneLoader.Instance.LoadScenes(_scenes[SceneEnum.Dungeon]);
            SoundManager.Instance.PlayMusic("dungeon_music");
        }

        public static void NextLevel()
        {
            // the Dungeon scene should be loaded at this point
            Assert.IsTrue(UnityEngine.SceneManagement.SceneManager.GetSceneByName(_scenes[SceneEnum.Dungeon]).IsValid());

            // unload the current Dungeon scene
            UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(_scenes[SceneEnum.Dungeon]);

            // Load the new Dungeon scene
            SceneLoader.Instance.LoadScenes(_scenes[SceneEnum.Dungeon]);
        }

        public static void BeginSandbox()
        {
            // the hub scene should be loaded at this point
            Assert.IsTrue(UnityEngine.SceneManagement.SceneManager.GetSceneByName(_scenes[SceneEnum.Hub]).IsValid());

            // unload the hub scene
            UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(_scenes[SceneEnum.Hub]);

            // Load the Dungeon scene
            SceneLoader.Instance.LoadScenes(_scenes[SceneEnum.Sandbox]);
            SoundManager.Instance.PlayMusic("dungeon_music");
        }


        public static void StartGame()
        {
            // TODO: state machine would make assertions like this unnecessary
            Assert.IsTrue(UnityEngine.SceneManagement.SceneManager.GetSceneByName(_scenes[SceneEnum.Title]).IsValid());

            // TODO: Await this
            // ? why does this need to be awaited again? we don't need to wait for the scene to be unloaded to load the next one
            UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(_scenes[SceneEnum.Title]);

            // Load the Hub
            SceneLoader.Instance.LoadScenes(_scenes[SceneEnum.Hub], _scenes[SceneEnum.HUD]);
            SoundManager.Instance.PlayMusic("hub_music");
        }

        public static void ExitDungeon()
        {
            // TODO: state machine would make assertions like this unnecessary
            Assert.IsTrue(UnityEngine.SceneManagement.SceneManager.GetSceneByName(_scenes[SceneEnum.Dungeon]).IsValid());

            // TODO: Await this
            // ? why does this need to be awaited again? we don't need to wait for the scene to be unloaded to load the next one
            UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(_scenes[SceneEnum.Dungeon]);

            // Load the Hub
            DataTracker.Instance.IsDungeon.Value = false;
            SceneLoader.Instance.LoadScenes(_scenes[SceneEnum.Hub]);
            SoundManager.Instance.PlayMusic("hub_music");
        }

        public static void ExitSandbox()
        {
            // TODO: state machine would make assertions like this unnecessary
            Assert.IsTrue(UnityEngine.SceneManagement.SceneManager.GetSceneByName(_scenes[SceneEnum.Sandbox]).IsValid());

            // TODO: Await this
            // ? why does this need to be awaited again? we don't need to wait for the scene to be unloaded to load the next one
            UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(_scenes[SceneEnum.Sandbox]);

            // Load the Hub
            SceneLoader.Instance.LoadScenes(_scenes[SceneEnum.Hub]);
            SoundManager.Instance.PlayMusic("hub_music");
        }

        public static void ExitGame()
        {
            Application.Quit();
        }

        public static void ReturnToTitle()
        {
            // unload all scenes except SampleScene and version
            int sceneCount = UnityEngine.SceneManagement.SceneManager.sceneCount;
            for (int i = 0; i < sceneCount; i++)
            {
                UnityEngine.SceneManagement.Scene scene = UnityEngine.SceneManagement.SceneManager.GetSceneAt(i);
                if (!IsStringInSceneEnumArray(scene.name, _alwaysLoadedScenes))
                {
                    UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(scene);
                }
            }

            // Load the Title scene
            DataTracker.Instance.IsDungeon.Value = false;
            SceneLoader.Instance.LoadScenes(_scenes[SceneEnum.Title]);
            SoundManager.Instance.PlayMusic("title_music");
        }

        // ---------------- private helper methods ----------------
        private static bool IsStringInSceneEnumArray(string str, SceneEnum[] array)
        {
            foreach (SceneEnum scene in array)
            {
                if (str == _scenes[scene])
                {
                    return true;
                }
            }
            return false;
        }
    }
}
