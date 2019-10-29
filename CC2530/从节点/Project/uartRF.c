#include "hal_defs.h"
#include "hal_cc8051.h"
#include "hal_int.h"
#include "hal_mcu.h"
#include "hal_board.h"
#include "hal_led.h"
#include "hal_rf.h"
#include "basic_rf.h"
#include "hal_uart.h" 
#include <stdio.h>
#include <string.h>
#include	<stdarg.h>

#define MAX_SEND_BUF_LEN  128
#define MAX_RECV_BUF_LEN  128
static uint8 pTxData[MAX_SEND_BUF_LEN]; //定义无线发送缓冲区的大小
static uint8 pRxData[MAX_RECV_BUF_LEN]; //定义无线接收缓冲区的大小

#define MAX_UART_SEND_BUF_LEN  128
#define MAX_UART_RECV_BUF_LEN  128
uint8 uTxData[MAX_UART_SEND_BUF_LEN]; //定义串口发送缓冲区的大小
uint8 uRxData[MAX_UART_RECV_BUF_LEN]; //定义串口接收缓冲区的大小
uint16 uTxlen = 0;
uint16 uRxlen = 0;


/*****点对点通讯地址设置******/
#define RF_CHANNEL                26           // 频道 11~26
#define PAN_ID                    0x1234      //网络id 
#define MY_ADDR                   0xffff      // 本机模块地址  1号模块
#define SEND_ADDR                 0x1111     //发送地址   1号模块

//#define MY_ADDR                   0x5678      // 本机模块地址  2号模块
//#define SEND_ADDR                 0x1234     //发送地址   2号模块

/**************************************************/
static basicRfCfg_t basicRfConfig;
/******************************************/
void MyByteCopy(uint8 *dst, int dststart, uint8 *src, int srcstart, int len)
{
    int i;
    for(i=0; i<len; i++)
    {
        *(dst+dststart+i)=*(src+srcstart+i);
    }
}
/****************************************************/
uint16 RecvUartData(void)
{   
    uint16 r_UartLen = 0;
    uint8 r_UartBuf[128]; 
    uRxlen=0; 
    r_UartLen = halUartRxLen();
    while(r_UartLen > 0)
    {
        r_UartLen = halUartRead(r_UartBuf, sizeof(r_UartBuf));
        MyByteCopy(uRxData, uRxlen, r_UartBuf, 0, r_UartLen);
        uRxlen += r_UartLen;
        halMcuWaitMs(5);   //这里的延迟非常重要，因为这是串口连续读取数据时候需要有一定的时间间隔
        r_UartLen = halUartRxLen();       
    }   
    return uRxlen;
}
/**************************************************/
// 无线RF初始化
void ConfigRf_Init(void)
{
    basicRfConfig.panId       =   PAN_ID;        //zigbee的ID号设置
    basicRfConfig.channel     =   RF_CHANNEL;    //zigbee的频道设置
    basicRfConfig.myAddr      =  MY_ADDR;   //设置本机地址
    basicRfConfig.ackRequest  =   TRUE;          //应答信号
    while(basicRfInit(&basicRfConfig) == FAILED); //检测zigbee的参数是否配置成功
    basicRfReceiveOn();                // 打开RF
}

void P2_Init()
{
  //初始化双联继电器
  P2SEL&=~0X01;
  P2DIR|=0X01;
  P1SEL&=~0X80;
  P1DIR|=0X80;
  P2_0=0;
  P1_7=0;
}

/********************MAIN************************/
void main(void)
{
    uint16 len = 0;
    halBoardInit();  //模块相关资源的初始化
    ConfigRf_Init(); //无线收发参数的配置初始化 
    P2_Init();
    halLedSet(3);
    halLedSet(4);
    halLedClear(1);
    halLedClear(2);
    while(1)
    { 
        if(basicRfPacketIsReady())   //查询有没收到无线信号
        {
            //接收无线数据
            len = basicRfReceive(pRxData, MAX_RECV_BUF_LEN, NULL);
            if(pRxData[0]==0xFF)
            {
              halLedSet(1);
              halLedSet(2);
              P2_0=1;
              P1_7=1;
              //开启双联继电器
            }
            if(pRxData[0]==0x00)
            {
              halLedClear(1);
              halLedClear(2);
              P2_0=0;
              P1_7=0;
              //关闭双联继电器
            }
        } 
    }
}
/************************main end *********************************************/