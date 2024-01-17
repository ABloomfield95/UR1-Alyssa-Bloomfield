#include	<string.h>

void *
memmove(void * d1, const void * s1, register size_t n)
{

	register char *		d;
	register const char *	s;

	s = s1;
	d = d1;
	
#if	defined(_PIC12) || defined(_PIC14) || defined(_PIC14E) || defined(_PIC14EX)
	if((unsigned short)s < (unsigned short)d && (unsigned short)s+n > (unsigned short)d) {
#else
	if(sizeof(s) == sizeof(d) && s < d && s+n > d) {		/* overlap? */
#endif
		s += n;
		d += n;
		do			/* n != 0 */
			*--d = *--s;
		while(--n);
	} else if(n)
		do
			*d++ = *s++;
		while(--n);
	return d1;
}
