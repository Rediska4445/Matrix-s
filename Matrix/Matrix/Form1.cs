using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Matrix
{
    public partial class Form1 : Form
    {
        Matrix matrix1;
        Matrix matrix2;

        Matrix result;

        int toNum;
        public Form1()
        {
            toNum = 0;
            BackColor = Color.Azure;
            TransparencyKey = Color.Azure;
            InitializeComponent();
            Dwm.WindowEnableBlurBehind(Handle);
            textBox1.Text = "2";
            textBox2.Text = "3";
            textBox3.Text = "2";
            textBox4.Text = "3";
        }

        private void button1_Click(object sender, EventArgs e)
        {
           SetMatrix(dataGridView1, int.Parse(textBox2.Text), int.Parse(textBox1.Text));
           matrix1 = new Matrix(int.Parse(textBox2.Text), int.Parse(textBox1.Text));
        }

        public void SetMatrix(DataGridView data, int rows, int cols)
        {
            data.Columns.Clear();
            data.Rows.Clear();
            for(int i = 0; i < cols; i++)
            {
                data.Columns.Add(i.ToString(), "");
            }
            for (int i = 0; i < rows; i++)
            {
                data.Rows.Add();
            }
        }

        public void Clear(DataGridView data)
        {
            for (int i = 0; i < data.Rows.Count; i++)
            {
                for(int i1 = 0; i1 < data.Rows[i].Cells.Count; i1++)
                {
                    data.Rows[i].Cells.Clear();
                }
            }
        }

        public void SetRandomize(DataGridView data, Random rand)
        {
            for (int i = 0; i < data.Rows.Count; i++)
            {
                for (int i1 = 0; i1 < data.Rows[i].Cells.Count; i1++)
                {
                    data.Rows[i].Cells[i1].Value = rand.Next(0, 50);
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SetMatrix(dataGridView2, int.Parse(textBox3.Text), int.Parse(textBox4.Text));
            matrix2 = new Matrix(int.Parse(textBox3.Text), int.Parse(textBox4.Text));
        }

        private void button4_Click(object sender, EventArgs e)
        {
            SetRandomize(dataGridView1, new Random());
        }

        private void SetResult(Matrix result)
        {
            SetMatrix(dataGridView3, result.GetMatrix().GetLength(0), result.GetMatrix().GetLength(1));

            for(int rows = 0; rows < result.GetMatrix().GetLength(1); rows++)
            {
                for (int cell = 0; cell < result.GetMatrix().GetLength(0); cell++)
                {
                    dataGridView3.Rows[rows].Cells[cell].Value = result.GetMatrix()[rows, cell];
                }
            }
        }

        private void Addition_Click(object sender, EventArgs e)
        {
            SetInsideMatrix();

            SetResult(matrix1.Add(matrix2));
        }

        private void button3_Click(object sender, EventArgs e)
        {
            dataGridView3.Rows.Clear();
            dataGridView3.Columns.Clear();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            for (int rows = 0; rows < dataGridView1.Rows.Count; rows++)
            {
                for (int cols = 0; cols < dataGridView1.Rows[rows].Cells.Count; cols++)
                {
                    matrix1.GetMatrix()[rows, cols] = int.Parse(dataGridView1.Rows[rows].Cells[cols].Value.ToString());
                }
            }

            for (int rows = 0; rows < dataGridView2.Rows.Count; rows++)
            {
                for (int cols = 0; cols < dataGridView2.Rows[rows].Cells.Count; cols++)
                {
                    matrix2.GetMatrix()[rows, cols] = -int.Parse(dataGridView2.Rows[rows].Cells[cols].Value.ToString());
                }
            }

            SetResult(matrix1.Add(matrix2));
        }

        private void SetInsideMatrix()
        {
            for (int rows = 0; rows < dataGridView1.Rows.Count; rows++)
            {
                for (int cols = 0; cols < dataGridView1.Rows[rows].Cells.Count; cols++)
                {
                    matrix1.GetMatrix()[rows, cols] = int.Parse(dataGridView1.Rows[rows].Cells[cols].Value.ToString());
                }
            }

            for (int rows = 0; rows < dataGridView2.Rows.Count; rows++)
            {
                for (int cols = 0; cols < dataGridView2.Rows[rows].Cells.Count; cols++)
                {
                    matrix2.GetMatrix()[rows, cols] = int.Parse(dataGridView2.Rows[rows].Cells[cols].Value.ToString());
                }
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            SetInsideMatrix();

            SetResult(matrix1.Multiply(matrix2));
        }

        private void button5_Click(object sender, EventArgs e)
        {
            SetRandomize(dataGridView2, new Random());
        }

        private void button8_Click(object sender, EventArgs e)
        {

        }

        private void button9_Click(object sender, EventArgs e)
        {
            SetMatrix(dataGridView2, int.Parse(textBox3.Text), int.Parse(textBox4.Text));
            matrix2 = new Matrix(int.Parse(textBox3.Text), int.Parse(textBox4.Text));
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            SetInsideMatrix();
            textBox5.Text = matrix1.GetDeterminate().ToString();
            textBox6.Text = matrix2.GetDeterminate().ToString();
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }
    }

    class Matrix
    {
        private int[,] matrix;

        public int rows;
        public int cols;

        private int determinate;

        public Matrix(int[,] matrix)
        {
            this.matrix = matrix;
            rows = matrix.GetLength(0);
            cols = matrix.GetLength(1);
        }

        public Matrix(int rows, int cols)
        {
            matrix = new int[rows, cols];
            rows = this.rows;
            cols = this.cols;
        }

        static double[][] MatrixInverse(double[][] matrix)
        {
            int n = matrix.Length;
            double[][] result = MatrixDuplicate(matrix);

            int[] perm;
            int toggle;
            double[][] lum = MatrixDecompose(matrix, out perm,
              out toggle);
            if (lum == null)
                throw new Exception("Unable to compute inverse");

            double[] b = new double[n];
            for (int i = 0; i < n; ++i)
            {
                for (int j = 0; j < n; ++j)
                {
                    if (i == perm[j])
                        b[j] = 1.0;
                    else
                        b[j] = 0.0;
                }

                double[] x = HelperSolve(lum, b);

                for (int j = 0; j < n; ++j)
                    result[j][i] = x[j];
            }
            return result;
        }

        static double[][] MatrixDuplicate(double[][] matrix)
        {
            // allocates/creates a duplicate of a matrix.
            double[][] result = MatrixCreate(matrix.Length, matrix[0].Length);
            for (int i = 0; i < matrix.Length; ++i) // copy the values
                for (int j = 0; j < matrix[i].Length; ++j)
                    result[i][j] = matrix[i][j];
            return result;
        }

        static double[] HelperSolve(double[][] luMatrix, double[] b)
        {
            // before calling this helper, permute b using the perm array
            // from MatrixDecompose that generated luMatrix
            int n = luMatrix.Length;
            double[] x = new double[n];
            b.CopyTo(x, 0);

            for (int i = 1; i < n; ++i)
            {
                double sum = x[i];
                for (int j = 0; j < i; ++j)
                    sum -= luMatrix[i][j] * x[j];
                x[i] = sum;
            }

            x[n - 1] /= luMatrix[n - 1][n - 1];
            for (int i = n - 2; i >= 0; --i)
            {
                double sum = x[i];
                for (int j = i + 1; j < n; ++j)
                    sum -= luMatrix[i][j] * x[j];
                x[i] = sum / luMatrix[i][i];
            }

            return x;
        }

        static double[][] MatrixDecompose(double[][] matrix, out int[] perm, out int toggle)
        {
            // Doolittle LUP decomposition with partial pivoting.
            // rerturns: result is L (with 1s on diagonal) and U;
            // perm holds row permutations; toggle is +1 or -1 (even or odd)
            int rows = matrix.Length;
            int cols = matrix[0].Length; // assume square
            if (rows != cols)
                throw new Exception("Attempt to decompose a non-square m");

            int n = rows; // convenience

            double[][] result = MatrixDuplicate(matrix);

            perm = new int[n]; // set up row permutation result
            for (int i = 0; i < n; ++i) { perm[i] = i; }

            toggle = 1; // toggle tracks row swaps.
                        // +1 -greater-than even, -1 -greater-than odd. used by MatrixDeterminant

            for (int j = 0; j < n - 1; ++j) // each column
            {
                double colMax = Math.Abs(result[j][j]); // find largest val in col
                int pRow = j;
                //for (int i = j + 1; i less-than n; ++i)
                //{
                //  if (result[i][j] greater-than colMax)
                //  {
                //    colMax = result[i][j];
                //    pRow = i;
                //  }
                //}

                // reader Matt V needed this:
                for (int i = j + 1; i < n; ++i)
                {
                    if (Math.Abs(result[i][j]) > colMax)
                    {
                        colMax = Math.Abs(result[i][j]);
                        pRow = i;
                    }
                }
                // Not sure if this approach is needed always, or not.

                if (pRow != j) // if largest value not on pivot, swap rows
                {
                    double[] rowPtr = result[pRow];
                    result[pRow] = result[j];
                    result[j] = rowPtr;

                    int tmp = perm[pRow]; // and swap perm info
                    perm[pRow] = perm[j];
                    perm[j] = tmp;

                    toggle = -toggle; // adjust the row-swap toggle
                }

                // --------------------------------------------------
                // This part added later (not in original)
                // and replaces the 'return null' below.
                // if there is a 0 on the diagonal, find a good row
                // from i = j+1 down that doesn't have
                // a 0 in column j, and swap that good row with row j
                // --------------------------------------------------

                if (result[j][j] == 0.0)
                {
                    // find a good row to swap
                    int goodRow = -1;
                    for (int row = j + 1; row < n; ++row)
                    {
                        if (result[row][j] != 0.0)
                            goodRow = row;
                    }

                    if (goodRow == -1)
                        throw new Exception("Cannot use Doolittle's method");

                    // swap rows so 0.0 no longer on diagonal
                    double[] rowPtr = result[goodRow];
                    result[goodRow] = result[j];
                    result[j] = rowPtr;

                    int tmp = perm[goodRow]; // and swap perm info
                    perm[goodRow] = perm[j];
                    perm[j] = tmp;

                    toggle = -toggle; // adjust the row-swap toggle
                }
                // --------------------------------------------------
                // if diagonal after swap is zero . .
                //if (Math.Abs(result[j][j]) less-than 1.0E-20) 
                //  return null; // consider a throw

                for (int i = j + 1; i < n; ++i)
                {
                    result[i][j] /= result[j][j];
                    for (int k = j + 1; k < n; ++k)
                    {
                        result[i][k] -= result[i][j] * result[j][k];
                    }
                }


            } // main j column loop

            return result;
        }

        public int GetDeterminate()
        {
            return GetDet(matrix, matrix.GetLength(0) != matrix.GetLength(1) ? 1 : matrix.GetLength(0));
        }

        private int GetDet(int[,] mat, int n)
        {

            // Base case: if the matrix is 1x1
            if (n == 1)
            {
                return mat[0, 0];
            }

            // Base case for 2x2 matrix
            if (n == 2)
            {
                return mat[0, 0] * mat[1, 1] -
                       mat[0, 1] * mat[1, 0];
            }

            // Recursive case for larger matrices
            int res = 0;
            for (int col = 0; col < n; col++)
            {

                // Create a submatrix by removing the first 
                // row and the current column
                int[,] sub = new int[n - 1, n - 1];
                for (int i = 1; i < n; i++)
                {
                    int subcol = 0;
                    for (int j = 0; j < n; j++)
                    {

                        // Skip the current column
                        if (j == col) continue;

                        // Fill the submatrix
                        sub[i - 1, subcol++] = mat[i, j];
                    }
                }

                // Cofactor expansion
                int sign = (col % 2 == 0) ? 1 : -1;
                res += sign * mat[0, col] * GetDet(sub, n - 1);
            }

            return res;
        }

        public Matrix Divide(Matrix toDiv)
        {
            if (matrix.GetLength(0) != toDiv.GetMatrix().GetLength(1) || matrix.GetLength(0) != toDiv.GetMatrix().GetLength(1))
                throw new ArithmeticException();

            Matrix result = new Matrix(matrix.GetLength(0), toDiv.GetMatrix().GetLength(1));

            if (GetDeterminate() == 0) throw new ArithmeticException(); 

            return result;
        }

        public Matrix Multiply(int toNum)
        {
            Matrix result = new Matrix(matrix.GetLength(0), matrix.GetLength(1));

            for (int x = 0; x < result.GetMatrix().GetLength(0); x++)
            {
                for (int y = 0; y < result.GetMatrix().GetLength(1); y++)
                {
                    result.GetMatrix()[x, y] *= toNum;        
                }
            }

            return result;
        }

        public Matrix Multiply(Matrix toMulti)
        { // 0 - rows, 1 - cols
            if (matrix.GetLength(0) != toMulti.GetMatrix().GetLength(1) || matrix.GetLength(0) != toMulti.GetMatrix().GetLength(1))
                throw new ArithmeticException();
            
            Matrix result = new Matrix(matrix.GetLength(0), toMulti.GetMatrix().GetLength(1));

            for(int x = 0; x < result.GetMatrix().GetLength(0); x++)
            {
                for (int y = 0; y < result.GetMatrix().GetLength(1); y++)
                {
                    for(int rows = 0; rows < matrix.GetLength(1); rows++)
                    {
                        result.GetMatrix()[x, y] += matrix[x, rows] * toMulti.GetMatrix()[rows, y];
                    }
                }
            }

            return result;
        }

        public Matrix Add(int add)
        {
            int[,] resBuff = new int[matrix.GetLength(0), matrix.GetLength(1)];
            Matrix result = new Matrix(resBuff);
            for(int x = 0; x < result.GetMatrix().GetLength(0); x++)
            {
                for (int y = 0; y < result.GetMatrix().GetLength(1); y++)
                {
                    result.GetMatrix()[x, y] += add;
                }
            }

            return result;
        }

        public Matrix Add(Matrix toAdd)
        {
            int[,] resBuff = new int[toAdd.GetMatrix().GetLength(0), matrix.GetLength(1)];
            Matrix result = new Matrix(resBuff);
            if (toAdd.GetMatrix().GetLength(0) != matrix.GetLength(0) || toAdd.GetMatrix().GetLength(1) != matrix.GetLength(1))
                throw new Exception();
            else
            {
                for (int i = 0; i < toAdd.GetMatrix().GetLength(0); i++)
                {
                    for (int i1 = 0; i1 < matrix.GetLength(1); i1++)
                    {
                        resBuff[i, i1] = matrix[i, i1] + toAdd.GetMatrix()[i, i1];
                    }
                }
            }

            return result;
        }

        public int[,] GetMatrix()
        {
            return matrix;
        }

    }

    [SuppressUnmanagedCodeSecurity]
    public class Dwm
    {
        public const int WM_DWMCOMPOSITIONCHANGED = 0x031E;

        public struct MARGINS
        {
            public int leftWidth;
            public int rightWidth;
            public int topHeight;
            public int bottomHeight;

            public MARGINS(int LeftWidth, int RightWidth, int TopHeight, int BottomHeight)
            {
                leftWidth = LeftWidth;
                rightWidth = RightWidth;
                topHeight = TopHeight;
                bottomHeight = BottomHeight;
            }

            public void SheetOfGlass()
            {
                leftWidth = -1;
                rightWidth = -1;
                topHeight = -1;
                bottomHeight = -1;
            }
        }

        [Flags]
        public enum DWM_BB
        {
            Enable = 1,
            BlurRegion = 2,
            TransitionOnMaximized = 4
        }

        public enum DWMWINDOWATTRIBUTE : uint
        {
            NCRenderingEnabled = 1,
            NCRenderingPolicy,
            TransitionsForceDisabled,
            AllowNCPaint,
            CaptionButtonBounds,
            NonClientRtlLayout,
            ForceIconicRepresentation,
            Flip3DPolicy,
            ExtendedFrameBounds,
            HasIconicBitmap,
            DisallowPeek,
            ExcludedFromPeek,
            Cloak,
            Cloaked,
            FreezeRepresentation,
            PassiveUpdateMode,
            UseHostBackDropBrush,
            AccentPolicy = 19,
            ImmersiveDarkMode = 20,
            WindowCornerPreference = 33,
            BorderColor,
            CaptionColor,
            TextColor,
            VisibleFrameBorderThickness,
            SystemBackdropType
        }

        public enum DWMNCRENDERINGPOLICY : uint
        {
            UseWindowStyle,
            Disabled,
            Enabled,
        };

        public enum DWMACCENTSTATE
        {
            ACCENT_DISABLED = 0,
            ACCENT_ENABLE_GRADIENT = 1,
            ACCENT_ENABLE_TRANSPARENTGRADIENT = 2,
            ACCENT_ENABLE_BLURBEHIND = 3,
            ACCENT_INVALID_STATE = 4
        }

        [Flags]

        public enum ThumbProperties_dwFlags : uint
        {
            RectDestination = 0x00000001,
            RectSource = 0x00000002,
            Opacity = 0x00000004,
            Visible = 0x00000008,
            SourceClientAreaOnly = 0x00000010
        }


        [StructLayout(LayoutKind.Sequential)]
        public struct AccentPolicy
        {
            public DWMACCENTSTATE AccentState;
            public int AccentFlags;
            public int GradientColor;
            public int AnimationId;

            public AccentPolicy(DWMACCENTSTATE accentState, int accentFlags, int gradientColor, int animationId)
            {
                AccentState = accentState;
                AccentFlags = accentFlags;
                GradientColor = gradientColor;
                AnimationId = animationId;
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct DWM_BLURBEHIND
        {
            public DWM_BB dwFlags;
            public int fEnable;
            public IntPtr hRgnBlur;
            public int fTransitionOnMaximized;

            public DWM_BLURBEHIND(bool enabled)
            {
                dwFlags = DWM_BB.Enable;
                fEnable = (enabled) ? 1 : 0;
                hRgnBlur = IntPtr.Zero;
                fTransitionOnMaximized = 0;
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct WinCompositionAttrData
        {
            public DWMWINDOWATTRIBUTE Attribute;
            public IntPtr Data;
            public int SizeOfData;

            public WinCompositionAttrData(DWMWINDOWATTRIBUTE attribute, IntPtr data, int sizeOfData)
            {
                Attribute = attribute;
                Data = data;
                SizeOfData = sizeOfData;
            }
        }

        [DllImport("dwmapi.dll")]
        internal static extern int DwmEnableBlurBehindWindow(IntPtr hwnd, ref DWM_BLURBEHIND blurBehind);

        [DllImport("dwmapi.dll")]
        internal static extern int DwmExtendFrameIntoClientArea(IntPtr hWnd, ref MARGINS pMarInset);

        [DllImport("dwmapi.dll")]
        internal static extern int DwmSetWindowAttribute(IntPtr hwnd, DWMWINDOWATTRIBUTE attr, ref int attrValue, int attrSize);

        [DllImport("dwmapi.dll")]
        internal static extern int DwmIsCompositionEnabled(ref int pfEnabled);

        [DllImport("User32.dll", SetLastError = true)]
        internal static extern int SetWindowCompositionAttribute(IntPtr hwnd, ref WinCompositionAttrData data);

        public static bool WindowSetAttribute(IntPtr hWnd, DWMWINDOWATTRIBUTE attribute, int attributeValue)
        {
            int result = DwmSetWindowAttribute(hWnd, attribute, ref attributeValue, sizeof(int));
            return (result == 0);
        }

        public static void Windows10EnableBlurBehind(IntPtr hWnd)
        {
            DWMNCRENDERINGPOLICY policy = DWMNCRENDERINGPOLICY.Enabled;
            WindowSetAttribute(hWnd, DWMWINDOWATTRIBUTE.NCRenderingPolicy, (int)policy);

            AccentPolicy accPolicy = new AccentPolicy()
            {
                AccentState = DWMACCENTSTATE.ACCENT_ENABLE_BLURBEHIND,
            };

            int accentSize = Marshal.SizeOf(accPolicy);
            IntPtr accentPtr = Marshal.AllocHGlobal(accentSize);
            Marshal.StructureToPtr(accPolicy, accentPtr, false);
            var data = new WinCompositionAttrData(DWMWINDOWATTRIBUTE.AccentPolicy, accentPtr, accentSize);

            SetWindowCompositionAttribute(hWnd, ref data);
            Marshal.FreeHGlobal(accentPtr);
        }

        public static bool WindowEnableBlurBehind(IntPtr hWnd)
        {
            DWMNCRENDERINGPOLICY policy = DWMNCRENDERINGPOLICY.Enabled;
            WindowSetAttribute(hWnd, DWMWINDOWATTRIBUTE.NCRenderingPolicy, (int)policy);

            DWM_BLURBEHIND dwm_BB = new DWM_BLURBEHIND(true);
            int result = DwmEnableBlurBehindWindow(hWnd, ref dwm_BB);
            return result == 0;
        }

        public static bool IsCompositionEnabled()
        {
            int pfEnabled = 0;
            int result = DwmIsCompositionEnabled(ref pfEnabled);
            return (pfEnabled == 1) ? true : false;
        }

        public static bool WindowBorderlessDropShadow(IntPtr hWnd, int shadowSize)
        {
            MARGINS margins = new MARGINS(0, shadowSize, 0, shadowSize);
            int result = DwmExtendFrameIntoClientArea(hWnd, ref margins);
            return result == 0;
        }
    }
}
