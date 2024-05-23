namespace Gameplay.UserModule
{
    public static class UserDataHelper
    {
        public static void Copy(this UserData oldData, UserData newData)
        {
            oldData.LastSpinIndex = newData.LastSpinIndex;
            oldData.SpinData = newData.SpinData;
        }
    }
}