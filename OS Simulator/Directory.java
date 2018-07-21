public class Directory
{
	private static int maxChars = 30; // max characters of each file name

	// Directory entries
	private int fsize[];        // each element stores a different file size.
	private char fnames[][];    // each element stores a different file name.
	private int maxFiles;   // max number of files

  	public Directory( int maxInumber ) // directory constructor
	{
		fsize = new int[maxInumber];     // maxInumber = max files
		for ( int i = 0; i < maxInumber; i++ )
			fsize[i] = 0;                 // all file size initialized to 0
		fnames = new char[maxInumber][maxChars];
		String root = "/";                // entry(inode) 0 is "/"
		fsize[0] = root.length( );        // fsize[0] is the size of "/".
		root.getChars( 0, fsize[0], fnames[0], 0 ); // fnames[0] includes "/"

		maxFiles = maxInumber;
  	}

  	public void bytes2directory( byte data[] )
  	{
      // assumes data[] received directory information from disk
      // initializes the Directory instance with this data[]
  		int offset = 0;
  		for (int i = 0; i < fsize.length; i++, offset += 4)
  			fsize[i] = SysLib.bytes2int(data, offset);

  		for (int i = 0; i < fnames.length; i++, offset += maxChars * 2)
  		{
  			String fname = new String(data, offset, maxChars * 2);
  			fname.getChars(0, fsize[i], fnames[i], 0);
  		}
  	}

   // new String(char[] value, int offset, int count)
      // Allocates a new String that contains characters from a subarray of the character array argument
      // offset argument is the index of the first character of the subarray
      // count argument specifies the length of the subarray
  	public byte[] directory2bytes( )
  	{
      // converts and return Directory information into a plain byte array
      // this byte array will be written back to disk
      // note: only meaningful directory information should be converted into bytes.

      // int2bytes(int i, byte[] b, int offset)
      // converts int i into 4 bytes, copy 4 bytes into b[offset], b[offset+1], b[offset+2], b[offset+3]
	int offset = 0;
	byte[] info = new byte[maxFiles * 60]; // 4? 64? maybe maxFiles need to be bigger
	for (int i = 0; i < maxFiles; i++, offset += 4)
	{
		SysLib.int2bytes(fsize[i], info, offset);
	}

	for (int i = 0; i < maxFiles; i++, offset += maxChars * 2)
	{
      	String fname = new String(fnames[i], 0, fsize[i]);
         // fsize[i] now has bytes allocated, need to move fname into fsize[i]
         // look for way to copy array into another array
         // https://docs.oracle.com/javase/6/docs/api/java/lang/String.html#String%28java.lang.String%29
         // https://www.thecrazyprogrammer.com/2016/05/copy-one-array-another-java.html
        // USE ABOVE #3 SYSTEM.ARRAYCOPY
         // https://docs.oracle.com/javase/7/docs/api/java/lang/String.html#getBytes()
      	byte[] temp = fname.getBytes();
         // info = Arrays.copyOf(temp, temp.length) MUST APPEND
         // https://stackoverflow.com/questions/5513152/easy-way-to-concatenate-two-byte-arrays
      	// info = info.concat(temp);
         // info = Bytes.concat(info, temp);
        System.arraycopy(temp, 0, info, offset, temp.length);
        // arraycopy(Object src, int srcPos, Object dest, int destPos, int length)
		}
		return info;
	}

	public short ialloc( String filename ) // IF FILENAME LONGER THAN #),  FAIL
	{
      // filename is the one of a file to be created.
      // allocates a new inode number for this filename

      // loop maxFiles to check where fsize value == 0
         // make fileName into chars
         // add length to fileSize
         // make fileName to char[]
         // add char[] into correct spot including offset
         // return true
		int fileNameLength = filename.length();
		if (fileNameLength > 30)
		{
			return -1;
		}

		for (short i = 0; i < maxFiles; i++)
		{
			int cur = fsize[i];
			if (cur == 0)
			{
				fsize[i] = fileNameLength;
				filename.getChars(0, fileNameLength, fnames[i], 0);
				// char[] temp = filename.toCharArray();
				// fnames[temp];
				return i;
			}
		}
		return -1;
	}

	public boolean ifree( short iNumber ) // DELETES FILENAME ARRAY
	{
      // deallocates this inumber (inode number)
      // the corresponding file will be deleted.

      // fileName to null, resize corresponding file size registered in directory to 0

        if (iNumber < maxFiles)
        {
            fsize[iNumber] = 0;
            //fnames[iNumber] = null;
            return true;
        }
        return false;
	}

	public short namei( String filename )
	{
      // returns the inumber corresponding to this filename
        for (short i = 0; i < maxFiles; i++)
        {
            String cur = new String(fnames[i], 0, fsize[i]);
            if (cur.equals(filename))
            {
                return i;
            }
        }
        return -1;
	}
}

/*
The "/" root directory maintains each file in a different directory entry that contains
its file name (maximum 30 characters; 60 bytes in Java) and the corresponding inode number.

The directory receives the maximum number of inodes to be created,
(i.e., thus the max. number of files to be created)
and keeps track of which inode numbers are in use.

Since the directory itself is considered as a file, its contents are maintained by an inode,
specifically inode 0. This can be located in the first 32 bytes of the disk block 1.

Upon booting ThreadOS, the file system instantiates the Directory class as the root directory through its constructor,
reads the file from the disk that can be found through the inode 0 at 32 bytes of the disk block 1,
and initializes the Directory instnace with the file contents.

Prior to shutdown, the file system must write back the Directory information onto the disk.

The methods bytes2directory( ) and directory2bytes will initialize the Directory instance with a byte array read
from the disk and converts the Directory instance into a byte array that will be thereafter written back to the disk.
*/