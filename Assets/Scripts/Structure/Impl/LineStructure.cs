namespace Structure.Impl {
    public class LineStructure : Structure {
        public void Awake() {
            Grid = new byte[,] {
                { 2, 3, 3, 3, 4 }
            };
        }
    }
}