#include <time.h>
#include "syscall.h"

int __clock_gettime(clockid_t, struct timespec *);

time_t time(time_t *t)
{
	if (t) {
		*t = -1LL;
	}
	return -1LL;
}
