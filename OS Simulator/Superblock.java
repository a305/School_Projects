import java.util.BitSet;

class SuperBlock
{
	private final int defaultInodeBlocks = 64;
	public int totalBlocks; // the number of disk blocks
	public int totalInodes; // the number of inodes
	public int freeList;    // the block number of the free list's head
	
	// Constructor
 	public SuperBlock( int diskSize ) // diskSize is number of disk blocks
	{
		// Read the SuperBlock from disk
		byte[] superBlock = new byte[Disk.blockSize];
		SysLib.rawread(0, superBlock);
		
		// Read stored SuperBlock variables in order from disk
		totalBlocks = SysLib.bytes2int(superBlock, 0);
		totalInodes = SysLib.bytes2int(superBlock, 4);
		freeList = SysLib.bytes2int(superBlock, 8);
		
		// Check for errors
		if (totalBlocks == diskSize && totalInodes > 0 && freeList >= 2)
			// Valid disk contents
			return;
		else {
			totalBlocks = diskSize;
			
			/* byte[] data = new byte[]
			SysLib.int2bytes((int)Math.ceil(((double)totalInodes * 32.0) / (double)Disk.blockSize) + 1)
			SysLib.rawwrite((int)Math.ceil(((double)totalInodes * 32.0) / (double)Disk.blockSize), data);
			
			// Mark blocks used by Superblock and Inodes as used.
			for (int i = 0; i < 1 + Math.ceil(((double)totalInodes * 32.0) / (double)Disk.blockSize); i++)
				freeListBlocks[i] = 1;
			
			// Mark all other blocks as unused.
			for (int i = (int)Math.ceil(((double)totalInodes * 32.0) / (double)Disk.blockSize); i < totalBlocks; i++)
				freeListBlocks[i] = 0;
			 */
			// Format disk
			SysLib.format(defaultInodeBlocks);
		}
	}

	// Writes SuperBlock data back to disk
	public void sync() {
		// Convert SuperBlock data elements to bytes and store in superBlock array
		byte[] superBlock = new byte[Disk.blockSize];
		SysLib.int2bytes(totalBlocks, superBlock, 0);
		SysLib.int2bytes(totalInodes, superBlock, 4);
		SysLib.int2bytes(freeList, superBlock, 8);
		
		// Write superBlock array to memory in block 0
		SysLib.rawwrite(0, superBlock);
	}
	
	// Retrieve and return current free block.
	public short getFreeBlock() {
		// If there are no free blocks left, return -1 for error.
		if (freeList == -1)
			return -1;
		
		// Store current free block to return.
		int curFreeBlock = freeList;
		
		// Read from free block
		byte[] data = new byte[Disk.blockSize];
		SysLib.rawread(freeList, data);
		
		// Get pointer bytes in block and set freeList pointer
		freeList = SysLib.bytes2int(data, 0);
		
		// Return current free block.
		return (short)curFreeBlock;
	}
	
	// Add given block to free list.
	public void returnBlock(int blockNumber) {
		// Write old freeList block into given block.
		byte[] data = new byte[Disk.blockSize];
		SysLib.int2bytes(freeList, data, 0);
		SysLib.rawwrite(blockNumber, data);
		
		// Set freeList to the given block.
		freeList = blockNumber;
	}
}
