#ifndef	_STRING_H
#define	_STRING_H

#ifdef __cplusplus
extern "C" {
#endif

#include <features.h>

#ifdef __cplusplus
#define NULL 0L
#else
#define NULL ((void*)0)
#endif

#define __NEED_size_t
#if defined(_POSIX_SOURCE) || defined(_POSIX_C_SOURCE) \
 || defined(_XOPEN_SOURCE) || defined(_GNU_SOURCE) \
 || defined(_BSD_SOURCE)
#define __NEED_locale_t
#endif

#include <bits/alltypes.h>

void *memcpy (void *__restrict, const void *__restrict, size_t);
void *memmove (void *, const void *, size_t);
void *memset (void *, int, size_t);
int memcmp (const void *, const void *, size_t);
#if defined (__AVR_CONST_DATA_IN_MEMX_ADDRESS_SPACE__)
const void *memchr(const void *, int, size_t);
#else
void *memchr (const void *, int, size_t);
#endif

char *strcpy (char *__restrict, const char *__restrict);
char *strncpy (char *__restrict, const char *__restrict, size_t);

char *strcat (char *__restrict, const char *__restrict);
char *strncat (char *__restrict, const char *__restrict, size_t);

int strcmp (const char *, const char *);
int strncmp (const char *, const char *, size_t);

int strcoll (const char *, const char *);
size_t strxfrm (char *__restrict, const char *__restrict, size_t);

#if defined (__AVR_CONST_DATA_IN_MEMX_ADDRESS_SPACE__)
const char *strchr(const char *, int);
const char *strrchr(const char *, int);
#else
char *strchr (const char *, int);
char *strrchr (const char *, int);
#endif

size_t strcspn (const char *, const char *);
size_t strspn (const char *, const char *);
#if defined (__AVR_CONST_DATA_IN_MEMX_ADDRESS_SPACE__)
const char *strpbrk(const char *, const char *);
const char *strstr(const char *, const char *);
#else
char *strpbrk (const char *, const char *);
char *strstr (const char *, const char *);
#endif
char *strtok (char *__restrict, const char *__restrict);

size_t strlen (const char *);

#if defined (__AVR_CONST_DATA_IN_MEMX_ADDRESS_SPACE__)
const char *strerror (int);
#else
char *strerror (int);
#endif

#if defined(_POSIX_SOURCE) || defined(_POSIX_C_SOURCE) \
 || defined(_XOPEN_SOURCE) || defined(_GNU_SOURCE) \
 || defined(_BSD_SOURCE)
char *strtok_r (char *__restrict, const char *__restrict, char **__restrict);
int strerror_r (int, char *, size_t);
char *stpcpy(char *__restrict, const char *__restrict);
char *stpncpy(char *__restrict, const char *__restrict, size_t);
size_t strnlen (const char *, size_t);
char *strdup (const char *);
char *strndup (const char *, size_t);
char *strsignal(int);
char *strerror_l (int, locale_t);
int strcoll_l (const char *, const char *, locale_t);
size_t strxfrm_l (char *__restrict, const char *__restrict, size_t, locale_t);
#endif

#if defined(_XOPEN_SOURCE) || defined(_GNU_SOURCE) \
 || defined(_BSD_SOURCE)
void *memccpy (void *__restrict, const void *__restrict, int, size_t);
#endif

#if defined(_GNU_SOURCE) || defined(_BSD_SOURCE)
char *strsep(char **, const char *);
size_t strlcat (char *, const char *, size_t);
size_t strlcpy (char *, const char *, size_t);
#endif

#ifdef _GNU_SOURCE
#define	strdupa(x)	strcpy(alloca(strlen(x)+1),x)
int strverscmp (const char *, const char *);
#if defined (__AVR_CONST_DATA_IN_MEMX_ADDRESS_SPACE__)
const char *strchrnul(const char *, int);
const char *strcasestr(const char *, const char *);
const void *memmem(const void *, size_t, const void *, size_t);
const void *memrchr(const void *, int, size_t);
#else
char *strchrnul(const char *, int);
char *strcasestr(const char *, const char *);
void *memmem(const void *, size_t, const void *, size_t);
void *memrchr(const void *, int, size_t);
#endif
void *mempcpy(void *, const void *, size_t);
#ifndef __cplusplus
char *basename();
#endif
#endif

#ifdef __cplusplus
}
#endif

#endif