public class Inode
{
	private final static int iNodeSize = 32;       // fix to 32 bytes
	private final static int directSize = 11;      // # direct pointers
	
	public int length;                             // file size in bytes
	public short count;                            // # file-table entries pointing to this
	public short flag;                             // 0 = unused, 1 = used, ...
	public short direct[] = new short[directSize]; // direct pointers
	public short indirect;                         // a indirect pointer

	// Default constructor
	Inode( )
	{
		length = 0;
		count = 0;
		flag = 0; // Was originally 1 in given code
		for ( int i = 0; i < directSize; i++ )
			direct[i] = -1;
		indirect = -1;
	}

	// Retrieval constructor
	Inode( short iNumber )
	{                 
		// Retrieve inode data from disk
		int blockNumber = 1 + iNumber / 16; // 16 inodes per block
		byte[] data = new byte[Disk.blockSize];
		SysLib.rawread(blockNumber, data);
		int offset = (iNumber % 16) * iNodeSize; // 16 inodes per block, 32 bytes per inode
	  
		// Retrieve and convert each stored inode data element to the correct data type
		length = SysLib.bytes2int(data, offset);
		offset += 4;
		count = SysLib.bytes2short(data, offset);
		offset += 2;
		flag = SysLib.bytes2short(data, offset);
		offset += 2;
		for (int i = 0; i < directSize; i++) {
			direct[i] = SysLib.bytes2short(data, offset);
			offset += 2;
		}
		indirect = SysLib.bytes2short(data, offset);
	}

	// Update function
	public void toDisk( short iNumber )
	{
		// Save inode data to the disk as the iNumber inode
		byte[] data = new byte[Disk.blockSize];
		int offset = (iNumber % 16) * iNodeSize; // Gets the offset based on the given iNumber (16 inodes per block, 32 bytes per inode)
  
		// Convert each inode data element to bytes and store in data array at the correct offset
		SysLib.int2bytes(length, data, offset);
		offset += 4;
		SysLib.short2bytes(count, data, offset);
		offset += 2;
		SysLib.short2bytes(flag, data, offset);
		for (int i = 0; i < directSize; i++) {
			SysLib.short2bytes(direct[i], data, offset);
			offset += 2;
		}
		SysLib.short2bytes(indirect, data, offset);
		
  
		// Store data array in memory in the correct block.
		int blockNumber = 1 + (iNumber / 16); // Gets the block based on the given iNumber (16 inodes per block)
		SysLib.rawwrite(blockNumber, data);
	}
	
	// Getter for the indirect pointer (may actually be getter for index inside indirect block)
	public short getIndexBlockNumber() {
		return indirect;
	}
	
	/* int getIndexBlockNumber(int entry, short offset){
        int target = entry/Disk.blockSize;

        if (target < directSize){
            if(direct[target] >= 0){
                return -1;
            }

            if ((target > 0 ) && (direct[target - 1 ] == -1)){
                return -2;
            }

            direct[target] = offset;
            return 0;
        }

        if (indirect < 0){
            return -3;
        }

        else{
            byte[] data = new byte[Disk.blockSize];
            SysLib.rawread(indirect,data);

            int blockSpace = (target - directSize) * 2;
            if ( SysLib.bytes2short(data, blockSpace) > 0){
                return -1;
            }
            else
            {
                SysLib.short2bytes(offset, data, blockSpace);
                SysLib.rawwrite(indirect, data);
            }
        }
        return 0;
    } */
	
	// Setter for the indirect pointer (may actually be setter for index inside indirect block)
	public boolean setIndexBlock(short indexBlockNumber) {
		indirect = indexBlockNumber;
		return true;
	}
	
	// Retrieves block that contains given offset for file represented by current inode.
	public short findTargetBlock(int offset) {
		// Get block pointer index
		int block = offset / Disk.blockSize;
		
		// If the block is larger than it's allowed to be, return -1 for error
		if (block >= Disk.blockSize / 2 + directSize)
			return -1;
		
		// If block is larger than 11 search index block, else return direct pointer for block.
		if (block >= directSize) {
			// Read from the the indirect block
			byte[] data = new byte[Disk.blockSize];
			SysLib.rawread(indirect, data);
			
			//for (int i = 0; i < Disk.blockSize / 2; i++): May need to change to this if looking for a particular number
			// Retrieve the indirect block containing the given offset
			return SysLib.bytes2short(data, (block - directSize) * 2);
		}
		else
			return direct[block];
	}
}
