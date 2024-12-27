namespace Structure.Impl {
    public class OZStructure : Structure {
        public void Awake() {
            Grid = new byte[,] {
                { 2, 3, 3, 9, 0 },
                { 0, 0, 0, 10, 4 }
            };
        }
    }
}