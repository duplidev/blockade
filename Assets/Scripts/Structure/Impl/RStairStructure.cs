namespace Structure.Impl {
    public class RStairStructure : Structure {
        public void Awake() {
            Grid = new byte[,] {
                { 5, 0, 0, 0 },
                { 10, 9, 0, 0 },
                { 0, 10, 9, 0 },
                { 0, 0, 10, 9 },
                { 0, 0, 0, 7 }
            };
        }
    }
}