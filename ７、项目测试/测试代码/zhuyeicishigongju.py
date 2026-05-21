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
        #点击贴子
        self._tz=(By.CSS_SELECTOR,"#NeiRong > div:nth-child(1) > a > div.card-list-right")
        #点赞
        self._dz=(By.CSS_SELECTOR, "#dianzan > input[type=button]")
        #点击评论
        self._pl=(By.CSS_SELECTOR, "#pinglun > input[type=button]")
        #输入框
        self._srk=(By.CSS_SELECTOR, "#app-hudong > div.bottom-input-fixed > input[type=text]:nth-child(1)")
        #发送
        self._fs=(By.CSS_SELECTOR, "#app-hudong > div.bottom-input-fixed > input[type=button]:nth-child(2)")
        #回复
        self._hf=(By.CSS_SELECTOR, "#app-hudong > div.comment-container > div > div:nth-child(4) > input[type=button]:nth-child(1)")
        #删除评论
        self._scpl = (By.CSS_SELECTOR,"#app-hudong > div.comment-container > div > div:nth-child(4) > input[type=button]:nth-child(2)")
        #返回上一页
        self._syy = (By.CSS_SELECTOR,"body > a")

    def a1(self,url):
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
    def dianjitzi(self):
        # 点击贴子
        a1 = self.wait.until(EC.element_to_be_clickable(self._tz))
        a1.click()
    def dianzan(self):
        a1 = self.wait.until(EC.element_to_be_clickable(self._dz))
        a1.click()
    def pinglun(self):
        a1 = self.wait.until(EC.element_to_be_clickable(self._pl))
        a1.click()
    def shurukuamng(self,gzy):
        #输入框
        a1=self.wait.until(EC.element_to_be_clickable(self._srk))
        a1.clear()
        a1.send_keys(gzy)
    def fasong(self):
        a1 = self.wait.until(EC.element_to_be_clickable(self._fs))
        a1.click()
    def huifu(self):
        # 回复
        a1 = self.wait.until(EC.element_to_be_clickable(self._hf))
        a1.click()
    def shanchuplun(self):
        a1 = self.wait.until(EC.element_to_be_clickable(self._scpl))
        a1.click()
    def shanyiyei(self):
        a1 = self.wait.until(EC.element_to_be_clickable(self._syy))
        a1.click()
