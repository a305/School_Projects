public class FileSystem {
	private SuperBlock superblock;
	private Directory directory;
	private FileTable filestable;
	
	private final int SEEK_SET = 0;
	private final int SEEK_CUR = 1;
	private final int SEEK_END = 2;
	
	public FileSystem(int diskBlocks) {
		// Create superblock, and format disk with 64 inodes in default.
		superblock = new SuperBlock(diskBlocks);
		
		// Create directory, and register "/" in directory entry 0.
		directory = new Directory(superblock.totalInodes);
		
		// File table is created, and store directory in file table.
		filestable = new FileTable(directory);
		
		// Read the "/" file from disk.
		FileTableEntry dirEnt = open("/", "r");
		int dirSize = fsize(dirEnt);
		if (dirSize > 0) {
			// The directory has some data.
			byte[] dirData = new byte[dirSize];
			read(dirEnt, dirData);
			directory.bytes2directory(dirData);
		}
		close(dirEnt);
	}
	
	public void sync() {
		
	}
	
	public int format(int files) 
    {
        /*
        The files argument passed to format indicates the maximum number of inodes. If files is 48, #inodes is 48. This means, 48/16 = 3 blocks will be allocated to maintain those 48 inodes. Then, you can see:
        block #0: superblock
        block #1: inodes 0 - 15
        block #2: inodes 16 -31
        block #3: inodes 32 -47
        block #4: the first free block
        */

        //delete here

        if(files > -1)
        {
            for(short i = 0; i < files; i++)
            {
                Inode inode = new Inode(i);
                inode.toDisk(i);
            }
            superblock.totalInodes = files;
            //superblock.totalBlocks = (int)Math.ceil((double)files/16.0);
			//superblock.freeList = 1 + (int)Math.ceil((double)superblock.totalInodes / (double)Disk.blockSize);
			superblock.freeList = 1 + (int)Math.ceil(superblock.totalInodes / 16.0);
            superblock.sync();
            directory = new Directory(superblock.totalBlocks);
            filestable = new FileTable(directory);
			
			// Initialize free block pointers
			//System.err.println(superblock.freeList);
			byte[] data = new byte[Disk.blockSize]; 
			for (int i = superblock.freeList; i < superblock.totalBlocks - 1; i++) {
				SysLib.int2bytes(i + 1, data, 0);
				SysLib.rawwrite(i, data);
			}
			
			// Special case for last free block
			SysLib.int2bytes(-1, data, 0);
			SysLib.rawwrite(superblock.totalBlocks - 1, data);
			
			return 0;
        }
        return -1;

    }
	
	public FileTableEntry open(String filename, String mode) {
		//System.err.println("Got to open");
		FileTableEntry ftEnt = filestable.falloc(filename, mode); // Change to int format
		//System.err.println("Made it past falloc");
		if (mode.equals("w")) {
			if (deallocAllBlocks(ftEnt) == false) // need to implement
				return null;
		}
		return ftEnt; // Need to return int
	}
	
	public int close(FileTableEntry FTE)
    {
        // Closes the file corresponding to fd
        // commits all file transactions on this file, 
        // and unregisters fd from the user file descriptor table of the calling thread's TCB. 

        // The return value is 0 in success, otherwise -1.
        synchronized (FTE)
        {
            FTE.count--;
            if (FTE.count < 0)
            {
                return -1;
            }
            if (FTE.count == 0)
            {
                if (filestable.ffree(FTE) == true)
                {
                    return 0;
                }
                return -1;
            }
            return 0;
        }

    }
	
	// Returns the size of the given file.
	public int fsize(FileTableEntry ftEnt) {
		return ftEnt.inode.length;
	}
	
	public int read(FileTableEntry ftEnt, byte[] buffer) {
		// If the buffer's length is longer than the amount of bytes between seekPtr and end of file, reduce length.
		int readLength = buffer.length;
		if (fsize(ftEnt) - ftEnt.seekPtr < buffer.length)
			readLength = fsize(ftEnt) - ftEnt.seekPtr;
		
		// Get number of bytes ahead of seekPtr in same block.
		int blockOffset = ftEnt.seekPtr % Disk.blockSize; // Possible error--------------------------------------------------------
		
		//System.err.println("Block offset: " + blockOffset + "Seek ptr: " + ftEnt.seekPtr);
		
		// Get number of blocks that need to be accessed.
		int numBlocks = (int)Math.ceil(((double)blockOffset + (double)readLength) / (double)Disk.blockSize);
		
		// 2D array to hold each block access.
		byte[][] data = new byte[numBlocks][Disk.blockSize];
		
		//Get starting block
		int startBlock = ftEnt.inode.findTargetBlock(ftEnt.seekPtr);
		int reads = 0;
		
		// For each number of  blocks to be read, read from the inode's direct pointers.
		for (int i = startBlock; i < Math.min(numBlocks, ftEnt.inode.direct.length); i++) {
			SysLib.rawread(ftEnt.inode.direct[i], data[i]);
			reads++;
		}
		
		// If the number of blocks exceed the direct pointers, read from the indirect blocks.
		if (numBlocks > reads && ftEnt.inode.indirect != -1) {
			byte[] indirectPtrs = new byte[Disk.blockSize];
			//System.err.println("Made it here");
			//if (ftEnt.inode.indirect == -1) while (true);
			SysLib.rawread(ftEnt.inode.indirect, indirectPtrs);
			
			// If direct blocks were read, start reading at indirect block 0, else read from the start block.
			int indirectStart;
			if (reads == 0)
				indirectStart = startBlock;
			else
				indirectStart = 0;
			
			// For each remaining block to be read, read from the indirect block's pointers
			for (int i = indirectStart; i < Math.min(numBlocks - reads, indirectPtrs.length / 2); i++) {
				SysLib.rawread(indirectPtrs[i + reads], data[reads]);
				reads++;
			}
		}
		
		int count = 0;
		
		// Append data from first read into buffer.
		for (int i = blockOffset; i < Math.min(buffer.length, data[0].length); i++) {
			buffer[count] = data[0][i];
			count++;
		}
		
		// Append data from middle reads into buffer.
		for (int i = 1; i < data.length - 1; i++) {
			for (int a = 0; a < data[0].length; a++) {
				buffer[count] = data[i][a];
				count++;
			}
		}
		
		// Only execute special last case if buffer hasn't already been completely read.
		if (count < buffer.length) {
			// Append data from last read into buffer.
			for (int i = 0; i < readLength - count; i++) {
				buffer[count] = data[data.length - 1][i];
				count++;
			}
		}
		
		// Increment seek pointer by number of bytes read.
		ftEnt.seekPtr += readLength;
		
		// Return number of bytes that have been read.
		return readLength;
	}
	
	public int write(FileTableEntry ftEnt, byte[] buffer) {
		// If write causes file to extend, record changes in inode
		if (ftEnt.seekPtr + buffer.length > ftEnt.inode.length) {
			// Get the number of blocks originally used by file.
			int origBlocks = (int)Math.ceil(ftEnt.inode.length / Disk.blockSize);
			int oldLength = ftEnt.inode.length;
			
			// Update the file's length the the new length necessary to contain buffer.
			ftEnt.inode.length = ftEnt.seekPtr + buffer.length;
			
			// Get the number of new blocks needed by file.
			int newBlocks = (int)Math.ceil(ftEnt.inode.length / Disk.blockSize);
			
			// Return -1 for error and reset file size if write exceeds maximum file size.
			if (newBlocks > (Disk.blockSize / 2) + ftEnt.inode.direct.length) {
				ftEnt.inode.length = oldLength;
				return -1;
			}
			int created = 0;
			
			// If there are any remaining empty direct block pointers remaining, allocate new blocks to these.
			if (ftEnt.inode.indirect == -1) {
				for (int i = origBlocks; i < ftEnt.inode.direct.length; i++) {
					ftEnt.inode.direct[i] = superblock.getFreeBlock();
					
					// Return -1 for error if write exceeds disk size.
					if (ftEnt.inode.direct[i] == -1)
						return -1;
					created++;
				}
			}
		
			// If more blocks are needed, start allocating indirect blocks.
			if (created < newBlocks - origBlocks) {
				// Array buffer to contain contents of indirect block.
				byte[] indirectBlock = new byte[Disk.blockSize];
				
				// If there is no indirect block, allocate a block to the inode's indirect pointer and initialize contents to 0.
				// Otherwise, read the contents of the current indirect block.
				if (ftEnt.inode.indirect == -1) {
					ftEnt.inode.indirect = superblock.getFreeBlock();
					
					// Return -1 for error if write exceeds disk size.
					if (ftEnt.inode.indirect == -1)
						return -1;
					for (int i = 0; i < Disk.blockSize; i++)
						indirectBlock[i] = 0;
				}
				else
					SysLib.rawread(ftEnt.inode.indirect, indirectBlock);
				
				// Set starting offset for indirect block write.
				int offset = origBlocks - ftEnt.inode.direct.length - 1;
				
				// Allocate new indirect blocks.
				for (int i = origBlocks - ftEnt.inode.direct.length - 1; i < (newBlocks - origBlocks) - created; i++) {
					SysLib.short2bytes(superblock.getFreeBlock(), indirectBlock, offset);
					
					// Return -1 for error if write exceeds disk size.
					if (indirectBlock[i] == -1)
						return -1;
					offset += 2;
				}
				
				// Write new blocks back to indirect.
				SysLib.rawwrite(ftEnt.inode.indirect, indirectBlock);
			}
			
			// Sync superblock to disk.
			superblock.sync();
			
			// Sync inode to disk.
			ftEnt.inode.toDisk(ftEnt.iNumber);
		}
		
		// Get the number of blocks to write
		int blockOffset = ftEnt.seekPtr % Disk.blockSize; // Get number of bytes ahead of seekPtr in same block.
		int numBlocks = (int)Math.ceil((blockOffset + (double)buffer.length) / (double)Disk.blockSize);
		
		// 2D array to hold each block access.
		byte[][] data = new byte[numBlocks][Disk.blockSize];
		
		//Get starting block
		int startBlock = ftEnt.inode.findTargetBlock(ftEnt.seekPtr);
		
		// Get original data from first block.
		SysLib.rawread(startBlock, data[0]);
		int count = 0;
		
		// Copy bytes from buffer into first block.
		 for (int i = blockOffset; i < Math.min(buffer.length, Disk.blockSize); i++) {
			data[0][i] = buffer[count];
			//System.err.println("Written: " + buffer[count]);
			count++;
		} 
		
		// Copy bytes from buffer into middle blocks.
		for (int i = 1; i < numBlocks - 1; i++) {
			for (int a = 0; a < Disk.blockSize; a++) {
				data[i][a] = buffer[count];
				count++;
			}
		}
		
		// Increment seek pointer by number of bytes written.
		ftEnt.seekPtr += buffer.length;
		
		// Get original data from last block.
		SysLib.rawread(ftEnt.inode.findTargetBlock(ftEnt.seekPtr), data[numBlocks - 1]);
		
		// If buffer has not already been completely written, execute special case for last block.
		if (count < buffer.length) {
			// Copy bytes from buffer into last block.
			for (int i = 0; i < buffer.length  - count; i++) {
				data[numBlocks - 1][i] = buffer[count];
				count++;
			}
		}
		
		int writes = 0;
	
		// For each number of  blocks to be written, write to the inode's direct pointers.
		for (int i = startBlock; i < Math.min(numBlocks, ftEnt.inode.direct.length); i++) {
			SysLib.rawwrite(ftEnt.inode.direct[i], data[i]);
			writes++;
		}
		
		// If the number of blocks exceed the direct pointers, write to the indirect blocks.
		if (numBlocks > writes && ftEnt.inode.indirect != -1) {
			byte[] indirectPtrs = new byte[Disk.blockSize];
			SysLib.rawread(ftEnt.inode.indirect, indirectPtrs);
			
			// If direct blocks were written, start writing at indirect block 0, else write from the start block.
			int indirectStart;
			if (writes == 0)
				indirectStart = startBlock;
			else
				indirectStart = 0;
			
			// For each remaining block to be written, write to the indirect block's pointers
			for (int i = indirectStart; i < Math.min(numBlocks - writes, indirectPtrs.length / 2); i++) {
				SysLib.rawwrite(indirectPtrs[i + writes], data[writes]); // TODO: Fix indirectPtrs incrementation
				writes++;
			}
		}
		
		// Return number of bytes that have been written.
		return buffer.length;
	}
	
	private boolean deallocAllBlocks(FileTableEntry ftEnt) {
		// Return all direct blocks to free list.
		for (int i = 0; i < ftEnt.inode.direct.length; i++)
			superblock.returnBlock(ftEnt.inode.direct[i]);
		
		// Check to see if there are indirect blocks.
		if (ftEnt.inode.indirect != -1) {
			byte[] data = new byte[Disk.blockSize];
			SysLib.rawread(ftEnt.inode.indirect, data);
			
			// Return all indirect blocks to free list.
			int offset = 0;
			for (int i = 0; i < (ftEnt.inode.length / Disk.blockSize) - ftEnt.inode.direct.length; i++) {
				superblock.returnBlock(SysLib.bytes2short(data, offset));
				offset += 2;
			}
			
			superblock.returnBlock(ftEnt.inode.indirect);
		}
		
		return true;
	}
	
	public int delete(String filename)
    {
        // Deletes the file specified by fileName.
        // All blocks used by file are freed.

        // If the file is currently open, it is not deleted and the operation returns a -1.
        // If successfully deleted a 0 is returned.

        FileTableEntry temp = open(filename, "w");
        if (close(temp) == 0)
        {
            if (directory.ifree(temp.iNumber) == true)
            {
                return 0;
            }
            return -1;
        }
        return -1;
    }
	
	public int seek(FileTableEntry ftEnt, int offset, int whence) {
		// If whence == SEEK_SET, file's seek pointer is set to beginning of file + offset
		if (whence == SEEK_SET)
			ftEnt.seekPtr = offset;
		// If whence == SEEK_CUR, file's seek pointer is set to current value + offset (offset can be positive or negative)
		else if (whence == SEEK_CUR)
			ftEnt.seekPtr = ftEnt.seekPtr + offset;
		// If whence == SEEK_END, file's seek pointer is set to size of file + offset (offset can be positive or negative)
		else if (whence == SEEK_END)
			ftEnt.seekPtr = (ftEnt.inode.length - 1) + offset;

		// If user attempts to set pointer to negative number, clamp it to zero.
		if (ftEnt.seekPtr < 0)
			ftEnt.seekPtr = 0;
		// If user attempts to set pointer beyond file size, clamp it to end of file.
		else if (ftEnt.seekPtr >= ftEnt.inode.length)
			ftEnt.seekPtr = ftEnt.inode.length - 1;
		
		return ftEnt.seekPtr;
	}
}
		