#include <string.h>

char *strcpy(char *restrict dest, const char *restrict src)
{
	const char *s = src;
	char *d = dest;
	while ((*d++ = *s++));
	return dest;
}
