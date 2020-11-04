namespace Cerberus.Logic.Games.T6.Assets
{
    public class T6XAsset : IAsset
    {
        private T6XAssetType _type;
        private int _ptr;

        public long GetHeaderPtr() => this._ptr;

        public int GetAssetType() => (int)this._type;

        public T6XAsset(T6XAssetType type, int ptr)
        {
            this._type = type;
            this._ptr = ptr;
        }
    }
}
