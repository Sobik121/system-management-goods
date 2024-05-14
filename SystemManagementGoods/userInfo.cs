namespace SystemManagementGoods
{
    class userInfo
    {
        private static string userName;
        public void SetName(string name)
        {
            userName = name;
        }

        public string GetName()
        {
            return userName;
        }
    }
}
