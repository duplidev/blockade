namespace Structure.Impl {
    public class DoubleStructure : Structure {
        public void Awake() {
            Grid = new byte[,] {
                { 5 },
                { 7 },
            };
        }
    }
}