using FreeTeam.BP.Common;
using FreeTeam.BP.Configuration;
using FreeTeam.BP.Data.Constants;
using FreeTeam.BP.ECS.Components;
using FreeTeam.BP.Extensions;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Leopotam.EcsLite.EventSystem;
using System;
using System.IO;
using UnityEngine;

namespace Assets.Scripts.ECS.Systems.Game
{
    public class ProfileSystem :
        IEcsPreInitSystem,
        IEcsInitSystem,
        IEcsDestroySystem,
        IEcsRunSystem
    {
        #region Inject
        private readonly EcsWorldInject world = default;

        private readonly EcsCustomInject<Configurations> config = default;
        #endregion

        #region Private
        private float saveDelay = 300f;
        private float saveTimer = 0f;

        private int saveRequestAmount = 3;
        private int saveRequestCounter = 0;
        #endregion

        #region Implementation
        public void PreInit(IEcsSystems systems)
        {
            saveDelay = config.Value.GetConstantFloat(ConstantKeys.PROFILE_SAVE_DELAY, saveDelay);
            saveRequestAmount = config.Value.GetConstantInt(ConstantKeys.PROFILE_SAVE_REQUEST_AMOUNT, saveRequestAmount);
        }

        public void Init(IEcsSystems systems)
        {
            ref var profileData = ref world.Value.AddUnique<ProfileData>();
            profileData.Value = LoadProfileData();
        }

        public void Destroy(IEcsSystems systems)
        {
            world.Value.DelUnique<ProfileData>();
        }

        public void Run(IEcsSystems systems)
        {
            ref var profileData = ref world.Value.GetUnique<ProfileData>();
            if (profileData.Value != null && profileData.Value.IsDirty)
            {
                if (saveTimer >= saveDelay || saveRequestCounter >= saveRequestAmount)
                {
                    SaveProfileData(profileData.Value);

                    saveTimer = 0;
                    saveRequestCounter = 0;
                }
            }

            saveTimer += Time.unscaledDeltaTime;
        }
        #endregion

        #region Private methods
        private FreeTeam.BP.Data.ProfileData LoadProfileData()
        {
            var profilePath = Path.Combine(Application.persistentDataPath, Paths.PROFILE_PATH);

            FreeTeam.BP.Data.ProfileData profile = null;

            if (File.Exists(profilePath))
            {
                try
                {
                    var profileJson = File.ReadAllText(profilePath);
                    profile = profileJson.Deserialize<FreeTeam.BP.Data.ProfileData>();
                    profile.SetDirtyWithoutNotify(false);

                    Debug.Log($"<b>[{GetType().Name}]</b> | {profile.UID} is loaded.");
                }
                catch (Exception exception)
                {
                    Debug.LogWarning($"<b>[{GetType().Name}]</b> | Load profile failed! {exception.Message}");
                }
            }

            return profile ?? new FreeTeam.BP.Data.ProfileData();
        }

        private void SaveProfileData(FreeTeam.BP.Data.ProfileData profile)
        {
            var profilePath = Path.Combine(Application.persistentDataPath, Paths.PROFILE_PATH);

            try
            {
                var profileJson = profile.Serialize(true);
                File.WriteAllText(profilePath, profileJson);

                profile.SetDirtyWithoutNotify(false);

                Debug.Log($"<b>[{GetType().Name}]</b> | {profile.UID} is saved.");
            }
            catch (Exception exception)
            {
                Debug.LogWarning($"<b>[{GetType().Name}]</b> | Save profile failed! {exception.Message}");
            }
        }
        #endregion
    }
}
