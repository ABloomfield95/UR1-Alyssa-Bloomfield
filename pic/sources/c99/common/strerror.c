#include <errno.h>
#include <string.h>
#include "libc.h"

#define E(a,b) ((unsigned char)a),
static const unsigned char errid[] = {
#include "__strerror.h"
};

#undef E
#define E(a,b) b "\0"
static const char errmsg[] =
#include "__strerror.h"
;

#define LCTRANS(a,b,c) (a)

char *strerror(int e)
{
	const char *s;
	int i;

	for (i=0; errid[i] && errid[i] != e; i++);
	for (s=errmsg; i; s++, i--) for (; *s; s++);
	return (char *)LCTRANS(s, LC_MESSAGES, loc);
}