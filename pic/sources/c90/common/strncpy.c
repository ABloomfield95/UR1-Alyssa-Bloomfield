#include	<string.h>

#ifdef _PIC16
__far char *
strncpy(register __far char * to, register const char * from, register size_t size)
#else /* _PIC16 */
char *
strncpy(register char * to, register const char * from, register size_t size)
#endif /* _PIC16 */
{

#ifdef _PIC16
	register __far char *	cp;
#else /* _PIC16 */
	register char *	cp;
#endif /* _PIC16 */

	cp = to;
	while(size) {
		size--;
		if(!(*cp++ = *from++))
			break;
	}
	while(size--)
		*cp++ = 0;
	return to;
}
