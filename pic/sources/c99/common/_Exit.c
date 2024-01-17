#include <stdlib.h>
#ifdef __syscall
#undef __syscall
#endif
#define __syscall(a,b)

_Noreturn void _Exit(int ec)
{
	__syscall(SYS_exit_group, ec);
	for (;;) __syscall(SYS_exit, ec);
}
