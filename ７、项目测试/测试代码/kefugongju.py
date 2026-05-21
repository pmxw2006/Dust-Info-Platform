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
        #处理问题
        self._clwt=(By.CSS_SELECTOR,"body > div > div.header > div:nth-child(1) > ul > li:nth-child(4) > a")
        #al输入框
        self._alsrk=(By.CSS_SELECTOR,"#feedback-app > div.main-panel > div.ai-toolbar > input")
        #al整理
        self._alzl=(By.CSS_SELECTOR,"#feedback-app > div.main-panel > div.ai-toolbar > button")
        #手动标记以处理
        self._sdbj=(By.CSS_SELECTOR,"#feedback-app > div.main-panel > div.feedback-list > div:nth-child(1) > div.feedback-actions > button")
        #点击取消
        self._djqx=(By.CSS_SELECTOR,"#feedback-app > div.mo-tai-zhe-zhao > div > div > button:nth-child(1)")
        #点击标记处理
        self._bjcl=(By.CSS_SELECTOR,"#feedback-app > div.mo-tai-zhe-zhao > div > div > button:nth-child(2)")
        #下一页
        self._xyy=(By.CSS_SELECTOR,"#feedback-app > div.main-panel > div.pagination > input[type=button]:nth-child(4)")
        #尾页
        self._wy= (By.CSS_SELECTOR, "#feedback-app > div.main-panel > div.pagination > input[type=button]:nth-child(5)")
        #上一页
        self._syy = (By.CSS_SELECTOR, "#feedback-app > div.main-panel > div.pagination > input[type=button]:nth-child(2)")
        #首页
        self._sy = (By.CSS_SELECTOR, "#feedback-app > div.main-panel > div.pagination > input[type=button]:nth-child(1)")

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
        #点击处理问题
        a1=self.wait.until(EC.element_to_be_clickable(self._clwt))
        a1.click()
    def a7(self,gzy):
        #al输入框
        a1=self.wait.until(EC.element_to_be_clickable(self._alsrk))
        a1.clear()
        a1.send_keys(gzy)
    def a8(self):
        #点击处理问题
        a1=self.wait.until(EC.element_to_be_clickable(self._alzl))
        a1.click()
    def a9(self):
        #点击手动标记
        a1=self.wait.until(EC.element_to_be_clickable(self._sdbj))
        a1.click()
    def a10(self):
        # 点击取消
        a1 = self.wait.until(EC.element_to_be_clickable(self._djqx))
        a1.click()
    def a11(self):
        # 点击确定
        a1 = self.wait.until(EC.element_to_be_clickable(self._bjcl))
        a1.click()
    def a12(self):
        # 点击下一页
        a1 = self.wait.until(EC.element_to_be_clickable(self._xyy))
        a1.click()
    def a13(self):
        # 点击尾页
        a1 = self.wait.until(EC.element_to_be_clickable(self._wy))
        a1.click()
    def a14(self):
        # 点击上一页
        a1 = self.wait.until(EC.element_to_be_clickable(self._syy))
        a1.click()
    def a15(self):
        # 首页
        a1 = self.wait.until(EC.element_to_be_clickable(self._sy))
        a1.click()

