using System.Text;

public class BigInteger
{
    public int[] _numbers;
    private string _status ;

    public BigInteger(string value)
    {
        _status = value.StartsWith("-") ? "-" : "";
        value = value.TrimStart('-');
        _numbers = value.Select(c => int.Parse(c.ToString())).Reverse().ToArray();
    }
    
    public override string ToString()
    {
        var res = string.Join("" , _numbers.Reverse()).TrimStart('0');
        return $"{_status}{res}";
    }
    

    public BigInteger Add(BigInteger another)
    {
        var status = "" ;
        if (_status == "-" && another._status == "-")
        {
            status = "-" ;
        }
        else if ( _status == "-")
        {
            return another.Sub(this);
        }
        else if (another._status == "-")
        {
            return this.Sub(another);
        }
        
        var reminder = 0;
        var result = new StringBuilder();
        for (int i = 0; i < (another._numbers.Length > _numbers.Length ? another._numbers.Length : _numbers.Length); i++)
        {
            int sum = reminder;
            if (i < _numbers.Length)
                sum += _numbers[i];
            if (i < another._numbers.Length)
                sum += another._numbers[i];

            result.Append(sum % 10); 
            reminder = sum / 10;  
        }

        if (reminder > 0)
            result.Append(reminder);
        
        var resultString = new string(result.ToString().Reverse().ToArray());
        var resultClass = new BigInteger(resultString);
        resultClass._status = status ;
        return resultClass;
    }
    
    public static BigInteger operator +(BigInteger a, BigInteger b) => a.Add(b);

    public BigInteger Sub(BigInteger another)
    {
        int[]? smaller;
        int[]? bigger;
        string status = "";

        if (_numbers.Length == another._numbers.Length)
        {
            int i = _numbers.Length - 1;
            while (i > 0 && _numbers[i] == another._numbers[i])
            {
                i--;
            }

            if (i == _numbers.Length)
            {
                return new BigInteger("0");
            }

            if (_numbers[i] > another._numbers[i])
            {
                bigger = _numbers;
                smaller = another._numbers;
            }
            
            else
            {
                smaller = _numbers;
                bigger = another._numbers;
                status = "-";
            }
        }
        else
        {
            if (_numbers.Length > another._numbers.Length)
            {
                bigger = _numbers;
                smaller = another._numbers;
            }
            else
            {
                smaller = _numbers;
                bigger = another._numbers;
                status = "-";
            }
        }

        var result = new StringBuilder();
        int borrow = 0;

        for (int i = 0; i < bigger.Length; i++)
        {
            int bigDigit = bigger[i] - borrow;
            int smallDigit = i < smaller.Length ? smaller[i] : 0;

            if (bigDigit < smallDigit)
            {
                bigDigit += 10;
                borrow = 1;
            }
            else
            {
                borrow = 0;
            }

            result.Append(bigDigit - smallDigit);
        }

        var resultString = new string(result.ToString().Reverse().ToArray());
        var final = new BigInteger(resultString);
        final._status = status;
        return final;
    }

    public static BigInteger operator -(BigInteger a, BigInteger b) => a.Sub(b);

    public BigInteger Multiply(BigInteger another)
    {
        var twoPow = NearestPowOf2(Math.Max(_numbers.Length, another._numbers.Length));
        _numbers = AddZeros(_numbers, twoPow);
        another._numbers = AddZeros(another._numbers, twoPow);

        return Karatsuba(this, another);
    }

    private BigInteger Karatsuba( BigInteger x, BigInteger y)
    {
        if (x._numbers.Length == 1 && y._numbers.Length == 1)
            return new BigInteger($"{x._numbers[0] * y._numbers[0]}");

        
        var a = new BigInteger(ReverseString(x._numbers.Skip(x._numbers.Length/2).ToArray()));
        var b = new BigInteger(ReverseString(x._numbers.Take(x._numbers.Length/2).ToArray()));
        var c = new BigInteger(ReverseString(y._numbers.Skip(y._numbers.Length/2).ToArray()));
        var d = new BigInteger(ReverseString(y._numbers.Take(y._numbers.Length/2).ToArray()));
        
        string ReverseString(int[] array)
        {
            Array.Reverse(array);
            return string.Join("" , array);
        }

        var ac = Karatsuba(a, c);
        var bd = Karatsuba(b, d);
        var AplusB = a + b;
        var CplusD = c + d;
        var abcd = AplusB.Multiply(CplusD);

        var ACplusBD = ac + bd;
        ac._numbers = Multiply10(ac._numbers, x._numbers.Length);
        var x0 = ac;
        var x1 = abcd - ACplusBD;
        x1._numbers = Multiply10( x1._numbers , x._numbers.Length/2);
        return x0 + x1 + bd;
    }

    private int[] Multiply10(int[] input, int start)
    {
        var array = new int[input.Length + start];
        for (int i = start; i < array.Length; i++)
        {
            array[i] = input[i - start];
        }

        return array;
    }
    
    private int NearestPowOf2(int number)
    {
        if (number < 1)
        {
            return 1;
        }

        int powerOfTwo = 1;
        while (powerOfTwo < number)
        {
            powerOfTwo *= 2;
        }
        return powerOfTwo;
    }
    
    private int[] AddZeros(int[] input, int num)
    {
        var result = new int[num];
        Array.Copy(input, result, input.Length);
        for (int i = input.Length  ; i < num; i++)
        {
            result[i] = 0;
        }
        return result;
    }
    
    public static BigInteger operator *(BigInteger a, BigInteger b) => a.Multiply(b); 
}