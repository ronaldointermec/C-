namespace GWSProject1.Modules
{
    public class OperationModel
    {
        public int Id { get; set; }
        public string? Message { get; set; }
        public int? Operator1 { get; set; }
        public OperationType Operation { get; set; }
        public int? Operator2 { get; set; }
        public float? Result { get; set; }
    }

    public enum OperationType
    {
        Add = 1,
        Subtract = 2,
        Multiply = 3,
        Divide = 4,
    }
}
