using System.IO;

namespace Cerberus.Logic.Games.T6.Assets
{
    public class T6GfxDrawSurface
    {
        private T6GfxDrawSurfaceFields _fields;
        public T6GfxDrawSurfaceFields GetFields() => this._fields;
        public bool IsPacked() => this._fields.HasData();

        public T6GfxDrawSurface(BinaryReader br)
        {
            this._fields = new T6GfxDrawSurfaceFields(br);
        }
    }
}