#include <stdlib.h>
#include <stdint.h>
#include "libc.h"

/* Ensure that at least 32 atexit handlers can be registered without malloc */
#define COUNT 32

static struct fl
{
	struct fl *next;
	void (*f[COUNT])(void *);
	void *a[COUNT];
} builtin, *head;

static int slot;

void __funcs_on_exit()
{
	void (*func)(void *), *arg;
	for (; head; head=head->next, slot=COUNT) while(slot-->0) {
		func = head->f[slot];
		arg = head->a[slot];
		func(arg);
	}
}

int __cxa_atexit(void (*func)(void *), void *arg, void *dso)
{
	/* Defer initialization of head so it can be in BSS */
	if (!head) head = &builtin;

	/* If the current function list is full, add a new one */
	if (slot==COUNT) {
		return -1;
	}

	/* Append function to the list. */
	head->f[slot] = func;
	head->a[slot] = arg;
	slot++;

	return 0;
}

static void call(void *p)
{
	((void (*)(void))(uintptr_t)p)();
}

int atexit(void (*func)(void))
{
	return __cxa_atexit(call, (void *)(uintptr_t)func, 0);
}