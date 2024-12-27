namespace Structure.Impl {
    public class Z2Structure : Structure {
        public void Awake() {
            Grid = new byte[,] {
                { 0, 0, 0, 5 },
                { 8, 3, 3, 11 },
                { 7, 0, 0, 0 }
            };
        }
    }
}