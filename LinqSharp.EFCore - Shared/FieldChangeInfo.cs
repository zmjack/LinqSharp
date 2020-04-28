namespace LinqSharp
{
    public class FieldChangeInfo
    {
        public string Display { get; set; }
        public bool IsModified { get; set; }
        public object Origin { get; set; }
        public object Current { get; set; }
    }
}
