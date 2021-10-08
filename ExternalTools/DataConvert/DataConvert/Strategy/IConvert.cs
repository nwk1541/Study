namespace DataConvert
{
    interface IConvert
    {
        void Init();
        void OnBeforeOperation();
        void Operation();
        void OnAfterOperation();
        void Export();

        void OnError();
    }
}
