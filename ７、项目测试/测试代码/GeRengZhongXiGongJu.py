from selenium.webdriver.support import expected_conditions as EC
from selenium.webdriver.common.by import By
from selenium.webdriver.support.wait import WebDriverWait
from selenium.webdriver.common.keys import Keys
import time


class Loginpage:
    def __init__(self, driver):
        self.driver = driver
        self.wait = WebDriverWait(driver, 10)  # 缩短至 10 秒，避免会话超时

        # 登录页元素
        self._zh = (By.ID, "TextBox1")
        self._mima = (By.ID, "TextBox2")
        self._dl = (By.CLASS_NAME, "denglu-anniu")
        self._jzmima = (By.ID, "CheckBox1")

        # 个人中心入口
        self._zhuyeidenglu = (By.CSS_SELECTOR, "body > div > div.header > div.geren > a:nth-child(1)")

        # 详细资料
        self._xiangxi = (By.CSS_SELECTOR, "#GeRenXx > p:nth-child(3) > a")
        self._bianjixinxi = (By.CLASS_NAME, "an-niu-ji-chu")

        # 编辑资料表单
        self._yonghuming = (By.CSS_SELECTOR,
                            "#saveForm > div.xin-xi-qu > div.xin-xi-wang-ge > div:nth-child(2) > input")
        self._yxiang = (By.CSS_SELECTOR, "#saveForm > div.xin-xi-qu > div.xin-xi-wang-ge > div:nth-child(3) > input")
        self._baocunbianji = (By.CLASS_NAME, "an-niu-cheng-gong")
        self._dianjquxiao = (By.CLASS_NAME, "an-niu-ya-se")

        # 错误提示（编辑资料）
        self._ZhangHuCiWuTiShi = (By.CSS_SELECTOR,
                                  "#saveForm > div.xin-xi-qu > div.xin-xi-wang-ge > div:nth-child(2) > span.tishi")
        self._yxiangciwutishi = (By.CSS_SELECTOR,
                                 "#saveForm > div.xin-xi-qu > div.xin-xi-wang-ge > div:nth-child(3) > span.tishi")

        # 修改密码页元素
        self._xgaimima = (By.CLASS_NAME, "tiao-zhuan-lian-jie")
        self._dangqianmima = (By.ID, "DangQianMiMa")
        self._xinmijma = (By.ID, "XinMiMa")
        self._qdinxinmima = (By.ID, "QueRenMiMa")
        self._bcmima = (By.CSS_SELECTOR, "#xiugaimima-app > div > form > div.an-niu-zu > button")

        # 当前密码的错误提示（前端校验）
        self._dangqianmima_tishi = (By.CSS_SELECTOR, "#xiugaimima-app div:nth-child(2) span.tishi")
        # 新密码的错误提示
        self._xinmimacwtixin = (By.CSS_SELECTOR, "#xiugaimima-app div:nth-child(3) span.tishi")
        # 确认密码的错误提示
        self._qrxinmimacwtx = (By.CSS_SELECTOR, "#xiugaimima-app div:nth-child(4) span.tishi")
        # 全局后端错误提示（多种常见样式）
        self._global_error_selectors = [
            "#xiugaimima-app .alert",
            "#xiugaimima-app .error-message",
            "#xiugaimima-app .toast",
            "#xiugaimima-app div[role='alert']",
            "#xiugaimima-app .error",
            "#xiugaimima-app .message",
            "body .alert",
            "body .error",
            ".error-tip",
            ".warning",
            ".message.error"
        ]

        self._fanhuishangyy = (By.CLASS_NAME, "fan-hui-lian-jie an-niu-fan-hui")
        self._fanhuigrzx = (By.CLASS_NAME, "fan-hui-lian-jie")

    # ------------------- 基础操作 -------------------
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

    def a03(self):
        self.wait.until(EC.element_to_be_clickable(self._jzmima)).click()

    def a4(self):
        self.wait.until(EC.element_to_be_clickable(self._dl)).click()

    def a5(self):
        self.wait.until(EC.element_to_be_clickable(self._zhuyeidenglu)).click()

    def a6(self):
        self.wait.until(EC.element_to_be_clickable(self._xiangxi)).click()

    def a7(self):
        self.wait.until(EC.element_to_be_clickable(self._bianjixinxi)).click()

    def a8(self, gzy):
        el = self.wait.until(EC.element_to_be_clickable(self._yonghuming))
        el.clear()
        if gzy == "":
            el.send_keys("a")
            el.send_keys(Keys.CONTROL, 'a')
            el.send_keys(Keys.DELETE)
            self.driver.execute_script(
                "arguments[0].dispatchEvent(new Event('input', { bubbles: true }));", el)
        else:
            el.send_keys(gzy)
        el.send_keys(Keys.TAB)

    def a9(self, gzy):
        el = self.wait.until(EC.element_to_be_clickable(self._yxiang))
        el.clear()
        if gzy == "":
            el.send_keys("a")
            el.send_keys(Keys.CONTROL, 'a')
            el.send_keys(Keys.DELETE)
            self.driver.execute_script(
                "arguments[0].dispatchEvent(new Event('input', { bubbles: true }));", el)
        else:
            el.send_keys(gzy)
        el.send_keys(Keys.TAB)

    def a10(self):
        self.wait.until(EC.element_to_be_clickable(self._baocunbianji)).click()

    def a11(self):
        self.wait.until(EC.element_to_be_clickable(self._dianjquxiao)).click()

    def a12(self):
        el = self.wait.until(EC.presence_of_element_located(self._ZhangHuCiWuTiShi))
        return el.text

    def a13(self):
        el = self.wait.until(EC.presence_of_element_located(self._yxiangciwutishi))
        return el.text

    def a14(self):
        self.wait.until(EC.element_to_be_clickable(self._fanhuishangyy)).click()

    def a15(self):
        self.wait.until(EC.element_to_be_clickable(self._xgaimima)).click()

    def a16(self, gzy):
        el = self.wait.until(EC.element_to_be_clickable(self._dangqianmima))
        el.clear()
        el.send_keys(gzy)

    def a17(self, gzy):
        el = self.wait.until(EC.element_to_be_clickable(self._xinmijma))
        el.clear()
        el.send_keys(gzy)

    def a18(self, gzy):
        el = self.wait.until(EC.element_to_be_clickable(self._qdinxinmima))
        el.clear()
        el.send_keys(gzy)

    def a19(self):
        self.wait.until(EC.element_to_be_clickable(self._bcmima)).click()

    def a20(self):
        """智能获取修改密码页的错误提示（兼容前后端所有错误类型）"""
        time.sleep(1)  # 等待错误信息出现

        # 1. 尝试通过预设的 CSS 选择器查找错误元素
        for selector in self._global_error_selectors:
            try:
                elements = self.driver.find_elements(By.CSS_SELECTOR, selector)
                for el in elements:
                    if el.is_displayed():
                        text = el.get_attribute("innerText").strip()
                        if text:
                            return text
            except:
                continue

        # 2. 尝试获取字段级错误提示
        try:
            cur_pwd_tip = self.driver.find_element(*self._dangqianmima_tishi)
            if cur_pwd_tip.is_displayed():
                text = cur_pwd_tip.get_attribute("innerText").strip()
                if text:
                    return text
        except:
            pass

        try:
            new_pwd_tip = self.driver.find_element(*self._xinmimacwtixin)
            if new_pwd_tip.is_displayed():
                text = new_pwd_tip.get_attribute("innerText").strip()
                if text:
                    return text
        except:
            pass

        # 3. 最后尝试从页面 body 中提取常见错误关键词
        try:
            body_text = self.driver.find_element(By.TAG_NAME, "body").text
            keywords = ["密码错误", "当前密码不正确", "密码不正确", "错误", "wrong password"]
            for kw in keywords:
                if kw in body_text:
                    return kw
        except:
            pass

        return ""

    def a21(self):
        """获取确认密码的错误提示"""
        try:
            el = self.wait.until(EC.presence_of_element_located(self._qrxinmimacwtx))
            return el.get_attribute("innerText").strip()
        except Exception:
            return ""

    def a22(self):
        self.wait.until(EC.element_to_be_clickable(self._fanhuigrzx)).click()

    def a23(self):
        return self.wait.until(EC.title_contains("个人中心"))

    def a24(self):
        return self.wait.until(EC.title_contains("微尘游戏咨询平台 - 登录"))