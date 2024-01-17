#include "stdio_impl.h"

off_t __stdio_seek(FILE *f, off_t off, int whence)
{
#ifndef STDIO_NO_FILE_IO
	off_t ret;
#ifdef SYS__llseek
	if (syscall(SYS__llseek, f->fd, off>>32, off, &ret, whence)<0)
		ret = -1;
#else
	ret = syscall(SYS_lseek, f->fd, off, whence);
#endif
	return ret;
#else
	return 0;
#endif
}
