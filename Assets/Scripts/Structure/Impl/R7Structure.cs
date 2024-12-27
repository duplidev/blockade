namespace Structure.Impl {
    public class R7Structure : Structure {
        public void Awake() {
            Grid = new byte[,] {
                { 8, 4 },
                { 6, 0 },
                { 7, 0 }
            };
        }
    }
}