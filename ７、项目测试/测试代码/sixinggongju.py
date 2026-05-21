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
        self._djtzi=(By.CSS_SELECTOR,"#NeiRong > div:nth-child(9) > a")
        #点击对方主页
        self._djdfzy=(By.CSS_SELECTOR,"#page-content > div > div.xiangqing-xinxi > span:nth-child(1) > a")
        #关注对方
        self._gzdf=(By.CSS_SELECTOR,"#yonghu > div > div > div > input")
        #点击私聊
        self._djsl=(By.CSS_SELECTOR,"#yonghu > div > div > div > input.si-liao")
        #点击对方聊天框
        self._dflt=(By.CSS_SELECTOR,"#HaoYou > div.ce-bian-lan > div.qun-liao-xiang")
        #消息框
        self._xxk=(By.CSS_SELECTOR,"#HaoYou > div.liaotian-qu > div.shuru-qu > input")
        #发送
        self._fs=(By.CSS_SELECTOR,"#HaoYou > div.liaotian-qu > div.shuru-qu > button")
        #点击私信
        self._sxin=(By.CSS_SELECTOR,"body > div > div.header > div:nth-child(1) > ul > li:nth-child(3) > a")
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
    def a6(self):
        # 点击贴子
        a1 = self.wait.until(EC.element_to_be_clickable(self._djtzi))
        a1.click()
    def a7(self):
        # 点击对方主页
        a1 = self.wait.until(EC.element_to_be_clickable(self._djdfzy))
        a1.click()
    def a8(self):
        # 点击关注对方
        a1 = self.wait.until(EC.element_to_be_clickable(self._gzdf))
        a1.click()
    def a9(self):
        # 点击私聊
        a1 = self.wait.until(EC.element_to_be_clickable(self._gzdf))
        a1.click()
    def a10(self):
        # 点击对方聊天
        a1 = self.wait.until(EC.element_to_be_clickable(self._dflt))
        a1.click()
    def a11(self,gzy):
        #消息框
        a1=self.wait.until(EC.element_to_be_clickable(self._xxk))
        a1.clear()
        a1.send_keys(gzy)
    def a12(self):
        # 发送
        a1 = self.wait.until(EC.element_to_be_clickable(self._fs))
        a1.click()
    def a13(self):
        #点击私信
        a1 = self.wait.until(EC.element_to_be_clickable(self._sxin))
        a1.click()
