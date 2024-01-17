#include "../time_impl.h"
#include <errno.h>

time_t mktime(struct tm *tm)
{
	struct tm new;
	long long t = __tm_to_secs(tm);

	if ((time_t)t != t) {
		goto error;
	}
	new.tm_isdst = 0;
	new.__tm_gmtoff = 0;
	new.__tm_zone = "GMT";
	if (__secs_to_tm(t, &new) < 0) {
		goto error;
	}
	*tm = new;
	return t;

error:
	errno = EOVERFLOW;
	return -1;
}
