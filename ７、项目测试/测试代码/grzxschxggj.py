from selenium.webdriver.support import expected_conditions as EC
from selenium.webdriver.common.by import By
from selenium.webdriver.support.wait import WebDriverWait

class Loginpage:
    def __init__(self, driver):
        self.driver = driver
        self.wait = WebDriverWait(driver, 10)

        self._zh = (By.ID, "TextBox1")
        self._mima = (By.ID, "TextBox2")
        self._dl = (By.CLASS_NAME, "denglu-anniu")
        self._grzx = (By.CSS_SELECTOR, "body > div > div.header > div.geren > a:nth-child(1)")
        self._xgtz = (By.CSS_SELECTOR, "#ZuoPing > div:nth-child(1) > div > a:nth-child(1)")
        self._biaoti = (By.CSS_SELECTOR, "#tiezi-edit-app > form > div:nth-child(3) > input")
        self._tupian = (By.CSS_SELECTOR, "#tianjia-app > form > div:nth-child(2) > div > div")
        self._tzineir = (By.CSS_SELECTOR, "#tiezi-edit-app > form > div:nth-child(5) > textarea")
        self._fabu = (By.CSS_SELECTOR, "#tiezi-edit-app > form > input.tijiao-an-niu")
        self._fanhuishangyy = (By.CSS_SELECTOR, "body > a")
        #点击删除
        self._shanc= (By.CSS_SELECTOR, "#ZuoPing > div:nth-child(1) > div > a:nth-child(2)")
        #确定删除
        self._qdshanc = (By.CSS_SELECTOR,"#customDelModal > div > div > button:nth-child(2)")
        #取消删除
        self._qxshanc = (By.CSS_SELECTOR,"#customDelModal > div > div > button:nth-child(1)")

    def a1(self, url):
        self.driver.get(url)
        self.wait.until(EC.presence_of_element_located(self._zh))

    def a2(self, gzy):
        el = self.wait.until(EC.element_to_be_clickable(self._zh))
        el.clear()
        el.send_keys(gzy)

    def a3(self, gzy):
        el = self.wait.until(EC.element_to_be_clickable(self._mima))
        el.clear()
        el.send_keys(gzy)

    def a5(self):
        self.wait.until(EC.element_to_be_clickable(self._dl)).click()

    def a6(self):
        self.wait.until(EC.element_to_be_clickable(self._grzx)).click()

    def a7(self):
        el = self.wait.until(EC.visibility_of_element_located(self._xgtz))
        el.click()

    def shurubiaoti(self, gzy):
        el = self.wait.until(EC.element_to_be_clickable(self._biaoti))
        el.clear()
        el.send_keys(gzy)

    def shuruzhangpian(self, gzy):
        el = self.wait.until(EC.element_to_be_clickable(self._tupian))
        el.clear()
        el.send_keys(gzy)

    def shuruneirong(self, gzy):
        el = self.wait.until(EC.element_to_be_clickable(self._tzineir))
        el.clear()
        el.send_keys(gzy)

    def fabu(self):
        self.wait.until(EC.element_to_be_clickable(self._fabu)).click()

    def fanhuishangyy(self):
        self.wait.until(EC.element_to_be_clickable(self._fanhuishangyy)).click()
    def a8(self):
        self.wait.until(EC.element_to_be_clickable(self._shanc)).click()
    def a9(self):
        self.wait.until(EC.element_to_be_clickable(self._qdshanc)).click()
    def a10(self):
        self.wait.until(EC.element_to_be_clickable(self._qxshanc)).click()
