namespace DataConvert
{
    interface IConvert
    {
        void Init();
        void OnBeforeOperation();
        string Operation();
        void OnAfterOperation();

        void OnError();
    }
}
