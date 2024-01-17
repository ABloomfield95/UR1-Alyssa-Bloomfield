#include <string.h>

size_t strcspn(const char *s, const char *c)
{
	register size_t	i;

	i = 0;
	while(*s && !strchr(c, *s)) {
		s++;
		i++;
	}
	return i;
}
