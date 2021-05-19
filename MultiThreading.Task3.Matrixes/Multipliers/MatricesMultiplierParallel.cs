using MultiThreading.Task3.MatrixMultiplier.Matrices;
using System.Threading.Tasks;

namespace MultiThreading.Task3.MatrixMultiplier.Multipliers
{
    public class MatricesMultiplierParallel : IMatricesMultiplier
    {
        private object _lockObj = new object();

        public IMatrix Multiply(IMatrix m1, IMatrix m2)
        {
            var resultMatrix = new Matrix(m1.RowCount, m2.ColCount);
            Parallel.For(0, m1.RowCount, i =>
            {
                Parallel.For(0, m2.ColCount, j =>
                {
                    long sum = 0;
                    Parallel.For(0, m1.ColCount, k =>
                    {
                        lock (_lockObj)
                        {
                            sum += m1.GetElement(i, k) * m2.GetElement(k, j);
                        }

                        resultMatrix.SetElement(i, j, sum);
                    });
                });
            });

            return resultMatrix;
        }
    }
}
