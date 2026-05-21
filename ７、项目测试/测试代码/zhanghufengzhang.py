from selenium.webdriver.support import expected_conditions as EC
from selenium.webdriver.common.by import By
from selenium.webdriver.support.wait import WebDriverWait
class Loginpage:
    def __init__(self,driver):
        #创建一个缓存方便调用
        self.driver = driver
        self.wait = WebDriverWait(driver,10)
        #账号
        self._zh=(By.ID,"TextBox1")
        #密码
        self._mima=(By.ID,"TextBox2")
        #登录
        self._dl=(By.CLASS_NAME,"denglu-anniu")
        #点击账户管理
        self._zhanghuguanli=(By.CSS_SELECTOR,"body > div > div.header > div:nth-child(1) > ul > li:nth-child(2) > a")
        #点击封禁一个账户
        self._fjzh=(By.CSS_SELECTOR,"#fengjin-app > div.table-wrapper > table > tbody > tr:nth-child(3) > td:nth-child(6) > button")
        #点击确定封禁
        self._qdfj=(By.CSS_SELECTOR,"#fengjin-app > div.mo-tai-zhe-zhao > div > div.mo-tai-an-niu > button:nth-child(2)")
        #用户名
        self._yhm=(By.CSS_SELECTOR,"#fengjin-app > div.sousuo-qu > input:nth-child(1)")
        #邮箱
        self._yx = (By.CSS_SELECTOR, "#fengjin-app > div.sousuo-qu > input:nth-child(2)")
        #用户id
        self._yhid = (By.CSS_SELECTOR, "#fengjin-app > div.sousuo-qu > input:nth-child(3)")
        #搜索
        self._ss=(By.CSS_SELECTOR,"#fengjin-app > div.sousuo-qu > button:nth-child(4)")
        #清空
        self._qk=(By.CSS_SELECTOR,"#fengjin-app > div.sousuo-qu > button:nth-child(5)")
        #取消
        self._qx=(By.CSS_SELECTOR,"#fengjin-app > div.mo-tai-zhe-zhao > div > div > button:nth-child(1)")
        #下一页
        self._xyy=(By.CSS_SELECTOR,"#FanYe > div > input:nth-child(4)")
        #尾页
        self._wy = (By.CSS_SELECTOR, "#FanYe > div > input:nth-child(5)")
        #首页
        self._sy=(By.CSS_SELECTOR,"#FanYe > div > input:nth-child(1)")
        #上一页
        self._syy=(By.CSS_SELECTOR,"#FanYe > div > input:nth-child(2)")
    def a1(self, url):
        #封装一个登录的方法
        self.driver.get(url)
        self.wait.until(EC.presence_of_element_located(self._zh))
    def a2(self,gzy):
        # 输入账号
       a1=self.wait.until(EC.element_to_be_clickable(self._zh))
       a1.clear()
       a1.send_keys(gzy)
    def a3(self,gzy):
        #输入密码
        a1=self.wait.until(EC.element_to_be_clickable(self._mima))
        a1.clear()
        a1.send_keys(gzy)
    def a5(self):
        #点击登录
        a1=self.wait.until(EC.element_to_be_clickable(self._dl))
        a1.click()
    def a6(self):
        #点击账户管理
        a1=self.wait.until(EC.element_to_be_clickable(self._zhanghuguanli))
        a1.click()
    def a7(self):
        #封禁其中一个贴子
        a1=self.wait.until(EC.element_to_be_clickable(self._fjzh))
        a1.click()
    def a8(self):
        #点击确定封禁
        a1=self.wait.until(EC.element_to_be_clickable(self._qdfj))
        a1.click()
    def a9(self):
        #点击确定取消
        a1=self.wait.until(EC.element_to_be_clickable(self._qx))
        a1.click()
    def a10(self,gzy):
        # 输入用户名
       a1=self.wait.until(EC.element_to_be_clickable(self._yhm))
       a1.clear()
       a1.send_keys(gzy)
    def a11(self,gzy):
        # 输入邮箱
       a1=self.wait.until(EC.element_to_be_clickable(self._yx))
       a1.clear()
       a1.send_keys(gzy)
    def a12(self,gzy):
        # 输入用户id
       a1=self.wait.until(EC.element_to_be_clickable(self._yhid))
       a1.clear()
       a1.send_keys(gzy)
    def a13(self):
        #点击确定取消
        a1=self.wait.until(EC.element_to_be_clickable(self._qx))
        a1.click()
    def a14(self):
        # 点击首页
        a1 = self.wait.until(EC.element_to_be_clickable(self._sy))
        a1.click()
    def a15(self):
        # 点击上一页
        a1 = self.wait.until(EC.element_to_be_clickable(self._syy))
        a1.click()
    def a16(self):
        # 点击尾页
        a1 = self.wait.until(EC.element_to_be_clickable(self._wy))
        a1.click()
    def a17(self):
        # 点击确定下一页
        a1 = self.wait.until(EC.element_to_be_clickable(self._xyy))
        a1.click()
    def a18(self):
        #点击确定封禁
        a1=self.wait.until(EC.element_to_be_clickable(self._ss))
        a1.click()






