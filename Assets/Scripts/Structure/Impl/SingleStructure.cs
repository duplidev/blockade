namespace Structure.Impl {
    public class SingleStructure : Structure {
        public void Awake() {
            Grid = new byte[,] {
                { 1 }
            };
        }
    }
}