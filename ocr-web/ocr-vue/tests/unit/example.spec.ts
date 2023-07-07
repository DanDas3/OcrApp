import { shallowMount } from "@vue/test-utils";
import OcrApp from "@/components/OcrApp.vue";

describe("OcrApp.vue", () => {
  it("renders props.msg when passed", () => {
    const msg = "new message";
    const wrapper = shallowMount(OcrApp, {
      propsData: { msg },
    });
    expect(wrapper.text()).toMatch(msg);
  });
});
