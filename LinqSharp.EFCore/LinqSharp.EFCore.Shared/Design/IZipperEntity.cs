namespace LinqSharp.EFCore.Design;

public interface IZipperEntity<TPoint> where TPoint : struct, IEquatable<TPoint>
{
    public TPoint ZipperStart { get; set; }
    public TPoint ZipperEnd { get; set; }
}
