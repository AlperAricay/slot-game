using System;
using Gameplay.SerializationModule;
using Gameplay.Utility;
using UnityEngine;

namespace Gameplay.UserModule
{
    public class UserDataController : IDisposable
    {
        private bool _hasFetchedData;
        
        private const string UserSavePath = "/UserData";

        private readonly UserData _userData;
        private readonly FileSerializationController _fileSerializationController;

        public UserDataController(UserData userData, FileSerializationController fileSerializationController)
        {
            _userData = userData;
            _fileSerializationController = fileSerializationController;

            SubscribeToEvents();
            FetchUserData();
        }
        
        private void FetchUserData()
        {
            var savedData =
                _fileSerializationController.DeserializeFromFile<UserData>(
                    Application.persistentDataPath + UserSavePath);
            
            var newData = savedData ?? new UserData();
            
            _userData.Copy(newData);
            _hasFetchedData = true;
        }

        private void OnApplicationFocusChanged(bool hasFocus)
        {
            if (!_hasFetchedData)
            {
                return;
            }
            
            if (!hasFocus)
            {
                Save();
            }
        }

        private void Save()
        {
            _fileSerializationController.SerializeToFile(_userData, Application.persistentDataPath + UserSavePath);
        }
        
        public void Dispose()
        {
            UnsubscribeFromEvents();
        }

        private void SubscribeToEvents()
        {
            ApplicationStateAnnouncer.OnApplicationFocusChangedEvent += OnApplicationFocusChanged;
        }

        private void UnsubscribeFromEvents()
        {
            ApplicationStateAnnouncer.OnApplicationFocusChangedEvent -= OnApplicationFocusChanged;
        }
    }
}