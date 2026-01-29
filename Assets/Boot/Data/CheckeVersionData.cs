using System;

namespace GamePlay.Boot
{
    [Serializable]
    public class CheckeVersionData
    {
        public string roomId;
        public string userId;
        public string appId;
        public string clientVersion; 
        public string token; 
        public bool debugtest; 
    }
}