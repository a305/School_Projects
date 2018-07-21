import java.util.StringTokenizer;

public class SysLib { public SysLib() {}
  
  public static int exec(String[] paramArrayOfString) { return Kernel.interrupt(1, 1, 0, paramArrayOfString); }
  

  public static int join()
  {
    return Kernel.interrupt(1, 2, 0, null);
  }
  
  public static int boot()
  {
    return Kernel.interrupt(1, 0, 0, null);
  }
  
  public static int exit()
  {
    return Kernel.interrupt(1, 3, 0, null);
  }
  
  public static int sleep(int paramInt)
  {
    return Kernel.interrupt(1, 4, paramInt, null);
  }
  
  public static int disk()
  {
    return Kernel.interrupt(2, 0, 0, null);
  }
  
  public static int cin(StringBuffer paramStringBuffer)
  {
    return Kernel.interrupt(1, 8, 0, paramStringBuffer);
  }
  
  public static int cout(String paramString)
  {
    return Kernel.interrupt(1, 9, 1, paramString);
  }
  
  public static int cerr(String paramString)
  {
    return Kernel.interrupt(1, 9, 2, paramString);
  }
  
  public static int rawread(int paramInt, byte[] paramArrayOfByte)
  {
    return Kernel.interrupt(1, 5, paramInt, paramArrayOfByte);
  }
  
  public static int rawwrite(int paramInt, byte[] paramArrayOfByte)
  {
    return Kernel.interrupt(1, 6, paramInt, paramArrayOfByte);
  }
  
  public static int sync()
  {
    return Kernel.interrupt(1, 7, 0, null);
  }
  
  public static int cread(int paramInt, byte[] paramArrayOfByte)
  {
    return Kernel.interrupt(1, 10, paramInt, paramArrayOfByte);
  }
  
  public static int cwrite(int paramInt, byte[] paramArrayOfByte)
  {
    return Kernel.interrupt(1, 11, paramInt, paramArrayOfByte);
  }
  
  public static int flush()
  {
    return Kernel.interrupt(1, 13, 0, null);
  }
  
  public static int csync()
  {
    return Kernel.interrupt(1, 12, 0, null);
  }
  
  public static String[] stringToArgs(String paramString)
  {
    StringTokenizer localStringTokenizer = new StringTokenizer(paramString, " ");
    String[] arrayOfString = new String[localStringTokenizer.countTokens()];
    for (int i = 0; localStringTokenizer.hasMoreTokens(); i++) {
      arrayOfString[i] = localStringTokenizer.nextToken();
    }
    return arrayOfString;
  }
  
  public static void short2bytes(short paramShort, byte[] paramArrayOfByte, int paramInt) {
    paramArrayOfByte[paramInt] = ((byte)(paramShort >> 8));
    paramArrayOfByte[(paramInt + 1)] = ((byte)paramShort);
  }
  
  public static short bytes2short(byte[] paramArrayOfByte, int paramInt) {
    short s = 0;
    s = (short)(s + (paramArrayOfByte[paramInt] & 0xFF));
    s = (short)(s << 8);
    s = (short)(s + (paramArrayOfByte[(paramInt + 1)] & 0xFF));
    return s;
  }
  
  public static void int2bytes(int paramInt1, byte[] paramArrayOfByte, int paramInt2) {
    paramArrayOfByte[paramInt2] = ((byte)(paramInt1 >> 24));
    paramArrayOfByte[(paramInt2 + 1)] = ((byte)(paramInt1 >> 16));
    paramArrayOfByte[(paramInt2 + 2)] = ((byte)(paramInt1 >> 8));
    paramArrayOfByte[(paramInt2 + 3)] = ((byte)paramInt1);
  }
  
  public static int bytes2int(byte[] paramArrayOfByte, int paramInt) {
    int i = ((paramArrayOfByte[paramInt] & 0xFF) << 24) + ((paramArrayOfByte[(paramInt + 1)] & 0xFF) << 16) + ((paramArrayOfByte[(paramInt + 2)] & 0xFF) << 8) + (paramArrayOfByte[(paramInt + 3)] & 0xFF);
    
    return i;
  }
  
  public static int format(int paramInt)
  {
    return Kernel.interrupt(1, 18, paramInt, null);
  }
  
  public static int open(String paramString1, String paramString2)
  {
    String[] arrayOfString = new String[2];
    arrayOfString[0] = paramString1;
    arrayOfString[1] = paramString2;
    return Kernel.interrupt(1, 14, 0, arrayOfString);
  }
  
  public static int close(int paramInt)
  {
    return Kernel.interrupt(1, 15, paramInt, null);
  }
  
  public static int read(int paramInt, byte[] paramArrayOfByte)
  {
    return Kernel.interrupt(1, 8, paramInt, paramArrayOfByte);
  }
  
  public static int write(int paramInt, byte[] paramArrayOfByte)
  {
    return Kernel.interrupt(1, 9, paramInt, paramArrayOfByte);
  }
  
  public static int seek(int paramInt1, int paramInt2, int paramInt3)
  {
    int[] arrayOfInt = new int[2];
    arrayOfInt[0] = paramInt2;
    arrayOfInt[1] = paramInt3;
    return Kernel.interrupt(1, 17, paramInt1, arrayOfInt);
  }
  
  public static int fsize(int paramInt)
  {
    return Kernel.interrupt(1, 16, paramInt, null);
  }
  
  public static int delete(String paramString)
  {
    return Kernel.interrupt(1, 19, 0, paramString);
  }
}
