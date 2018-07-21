import java.util.*;

public class FileTable
{

   private Vector table;         // the actual entity of this file table
   private Directory dir;        // the root directory 
   
   final int UNUSED = 0;
   final int USED = 1;
   final int READ = 2;
   final int WRITE = 3;
   final int TO_BE_DELETED = 4;
   

   public FileTable( Directory directory ) // constructor
   {
      table = new Vector( );     // instantiate a file (structure) table
      dir = directory;           // receive a reference to the Director
   }                             // from the file system

   // major public methods
   public synchronized FileTableEntry falloc( String filename, String mode )
   {
	   short iNumber = -1;
	   Inode inode = null;

	   iNumber = (filename.equals("/")) ? 0 : dir.namei(filename);

	   while(true)
	   {
	   		if(iNumber >= 0)
		   {
		   		inode = new Inode(iNumber);
		   		if(!mode.equals("r"))  //mode is w/w+/a
		   		{
		   			if(inode.flag == UNUSED)  //file currently not being used
		   			{
		   				inode.flag = WRITE;
		   				break;
		   			}
		   			else  //inode currently being used
		   			{
		   				if(inode.flag == TO_BE_DELETED)
		   				{
		   					iNumber = -1;
		   					return null;
		   				}
		   				else
		   				{
		   					// file currently being altered/read and trying to w/w+/a
		   					try
			   				{
			   					wait();
			   				}
			   				catch(InterruptedException e){}
		   				}
		   			}
		   		} 
		   		else   //trying to read
		   		{
		   			if(inode.flag == TO_BE_DELETED)
		   			{
		   				iNumber = -1;
		   				return null;

		   			}
		   			else if(inode.flag == READ || inode.flag == UNUSED)
		   			{
		   				break; //trying to read and file is being read
		   			}
		   			else
		   			{
		   				//trying to read and file is being altered
		   				try
		   				{
		   					wait();
		   				}
		   				catch(InterruptedException e){}
		   			}
		   		}
		   	}
		    else
		    {
		   		//iNumber is negative, file does not exist
		   		if(mode.equals("r"))   //mode is read
		   		{	//no such file exists, there's nothing to read
		   			return null;
		   		}
		   		else  //mode is write or append
		   		{
		   			iNumber = dir.ialloc(filename);
		   			inode = new Inode();
		   		}
		    }
	   }
	   
	   
	   inode.count++;
	   inode.toDisk(iNumber);
	   FileTableEntry e = new FileTableEntry(inode, iNumber, mode);
	   table.addElement(e);  //create a table entry and register it
	   
	   return e;
	   
	   
	   
      
      // allocate/retrieve and register the corresponding inode using dir
      // increment this inode's count
      // immediately write back this inode to the disk
      // return a reference to this file (structure) table entry
   }

   public synchronized boolean ffree( FileTableEntry e )
   {

        if(table.indexOf(e) == -1)  //entry doesn't exist in table
        {
        	return false;
        }

        e.inode.toDisk(e.iNumber);

        e.inode.count--;
        if(e.inode.count == 0)
        {
        	e.inode.flag = UNUSED;
        }

        table.remove(table.indexOf(e));
        notifyAll();

        return true;

      // receive a file table entry reference
      // save the corresponding inode to the disk
      // free this file table entry.
      // return true if this file table entry found in my table

   		// remember to decrement the node count
   		// if count is 0, mark it as unused 
   }

   public synchronized boolean fempty( )
   {
      return table.isEmpty( );  // return if table is empty 
   }                            // should be called before starting a format
}



