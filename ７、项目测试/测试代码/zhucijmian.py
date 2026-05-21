from selenium.webdriver.support import expected_conditions as EC
from selenium.webdriver.common.by import By
from selenium.webdriver.support.wait import WebDriverWait
class zhucifangfa:
    def __init__(self,driver):
        self.driver = driver
        self.wait = WebDriverWait(self.driver,10)
        self._yhm=(By.ID,"YongHuMing")#用户名
        self._yxh=(By.ID,"YouXiang")#邮箱
        self._mima=(By.ID,"MiMa")#密码
        self._qrmm=(By.ID,"QueRenMiMa")#确定密码
        self._tjzh=(By.CSS_SELECTOR,"#tianjia-app > form > div.denglu-zhuti > div.AnNiuQuYu > button")#添加账户
        self._fhzh=(By.CLASS_NAME,"AnNiuFanHui")#返回列表
        #账户为空提醒
        self._cwtx=(By.CSS_SELECTOR,"#tianjia-app > form > div.denglu-zhuti > div:nth-child(1) > span:nth-child(3) > span")
        #邮箱为空提醒
        self._yxwktx=(By.CSS_SELECTOR,"#tianjia-app > form > div.denglu-zhuti > div:nth-child(2) > span:nth-child(3) > span")
        #密码为空提醒
        self._mimatx=(By.CSS_SELECTOR,"#tianjia-app > form > div.denglu-zhuti > div:nth-child(3) > span.tishi > span")
        #确定密码提醒
        self._qdmm=(By.CSS_SELECTOR,"#tianjia-app > form > div.denglu-zhuti > div:nth-child(4) > span:nth-child(3) > span")
        #跳转消息提醒
        self._tzxx=(By.CSS_SELECTOR,"#tianjia-app > form > div.denglu-zhuti > div.cuowu-tishi")
    def a1(self,url):#封装登录注册页的方法
        self.driver.get(url)
        self.wait.until(EC.presence_of_element_located(self._yhm))
    def a2(self,gzy):#输入用户名
        a1 = self.wait.until(EC.element_to_be_clickable(self._yhm))
        a1.clear()
        a1.send_keys(gzy)
    def a3(self,gzy):#输入邮箱
        a1 = self.wait.until(EC.element_to_be_clickable(self._yxh))
        a1.clear()
        a1.send_keys(gzy)
    def a4(self,gzy):#输入密码
        a1 = self.wait.until(EC.element_to_be_clickable(self._mima))
        a1.clear()
        a1.send_keys(gzy)
    def a5(self,gzy):#输入确定密码
        a1 = self.wait.until(EC.element_to_be_clickable(self._qrmm))
        a1.clear()
        a1.send_keys(gzy)
    def a6(self):#添加用户
        a1 = self.wait.until(EC.element_to_be_clickable(self._tjzh))
        a1.click()
    def a7(self):#返回列表
        a1 = self.wait.until(EC.element_to_be_clickable(self._fhzh))
        a1.click()
    def a8(self):#返回账户为空的或错误本提醒
        a1=self.wait.until(EC.visibility_of_element_located(self._cwtx))
        return a1.text
    def a9(self):#返回邮箱为空的文本提醒
        a1=self.wait.until(EC.visibility_of_element_located(self._yxwktx))
        return a1.text
    def a10(self):#返回密码为空的文本提醒
        a1=self.wait.until(EC.visibility_of_element_located(self._mimatx))
        return a1.text
    def a11(self):#返回密码为空的文本提醒
        a1=self.wait.until(EC.visibility_of_element_located(self._qdmm))
        return a1.text
    def a12(self):#返回跳转的文本提醒
        a1=self.wait.until(EC.visibility_of_element_located(self._tzxx))
        return a1.text
    def a13(self):#跳转成功提醒
        return self.wait.until(EC.title_contains("添加账户 - 后台管理系统"))
    def a14(self):#跳转成功提醒
        return self.wait.until(EC.title_contains("微尘游戏咨询平台 - 登录"))


