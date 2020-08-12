// RISC-V Compliance Test Header File
// Copyright (c) 2017, Codasip Ltd. All Rights Reserved.
// See LICENSE for license details.
//
// Description: Common header file for RV32I tests

#ifndef _COMPLIANCE_TEST_H
#define _COMPLIANCE_TEST_H

#include "riscv_test.h"

//-----------------------------------------------------------------------
// RV Compliance Macros
//-----------------------------------------------------------------------

#define RV_COMPLIANCE_HALT                                                    \
        testend: j testend                                                    \

#define RV_COMPLIANCE_RV32M                                                   \
        RVTEST_RV32M                                                          \

#define RV_COMPLIANCE_CODE_BEGIN                                              \
        /*RVTEST_CODE_BEGIN*/                                                     \

#define RV_COMPLIANCE_CODE_END                                                \
        /*RVTEST_CODE_END*/                                                       \

#define RV_COMPLIANCE_DATA_BEGIN                                                \
        RVTEST_DATA_BEGIN                                                     \

#define RV_COMPLIANCE_DATA_END                                                \
        RVTEST_DATA_END                                                       \
data_marker:                                                                    \
        .fill 2, 4, 0x87654321;                                                 \

#endif
