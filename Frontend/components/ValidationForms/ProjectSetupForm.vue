<script setup lang="ts">
import { useForm } from 'vee-validate';
import { object, string } from 'zod';
import { toTypedSchema } from '@vee-validate/zod';
import type { CampaignType } from '~/core/direct-mail-api-client';
import type { IAlert } from '~/core/alert.interfaces';

interface IField {
  value: string,
  error: string,
}

interface IProps {
  productId: string,
};
const props = defineProps<IProps>();

const projectsStore = useProjectStore();
const integratedProductsStore = useIntegratedProductsStore();

projectsStore.fetchProjectTypes();

const projectTypesList = computed(() => projectsStore.getProductTypes);

const projectTypeField: Ref<IField> = ref({ 
  value: '',
  error: '',
});

const titleField: Ref<IField> = ref({ 
  value: '',
  error: '',
});

const ObjectSchema = object({
  title: string().default(''),
});
const { handleSubmit } = useForm({
  validationSchema: toTypedSchema(ObjectSchema),
});

const submitProject = handleSubmit(async (values) => {
  titleField.value.value = values.title;

  if (checkValidation()) return;
  
  const payloadCreateProject: { title: string, type: CampaignType } = {
    title: titleField.value.value,
    type: projectTypeField.value.value as CampaignType,
  };

  await integratedProductsStore.updateProductResources({ id: props.productId });

  const lineItemId = await createProject(payloadCreateProject);
  navigateTo(`/create-project/choose-template?lineItemId=${lineItemId}`);
});

function requiredField(field: IField): boolean {
  if (field.value.trim()) {
    field.error = '';
    return true;
  } else {
    field.error = 'Required';
    return false;
  }
};

function updateFieldValue(value: string) {
  projectTypeField.value.value = value;
  requiredField(projectTypeField.value);
}

function validateProduct(alertList: IAlert[]): boolean {
  const hasProduct: boolean = !!props.productId.trim() === false;
  if (hasProduct) {
    alertList.push({
      label: 'Validation error',
      message: 'Product is required',
    });
  }
  return hasProduct;
}

function validateCampaignType(alertList: IAlert[]): boolean {
  const hasProjectType: boolean = requiredField(projectTypeField.value) === false;
  if (hasProjectType) {
    alertList.push({
      label: 'Validation error',
      message: 'Project type is required',
    });
  }
  return hasProjectType;
}

function validateTitle(alertList: IAlert[]): boolean {
  const isValidTitle: boolean = requiredField(titleField.value) === false;
  if (isValidTitle) {
    alertList.push({
      label: 'Validation error',
      message: 'Title must not be empty',
    });
  }
  return isValidTitle;
}

function checkValidation(): boolean {
  const alertList: IAlert[] = [];
  
  const hasProduct = validateProduct(alertList);
  const hasProjectType = validateCampaignType(alertList);
  const isValidTitle = validateTitle(alertList);

  if (alertList.length) {
    ShowAlert(alertList);
  }
  
  return hasProjectType || hasProduct || isValidTitle;
};

async function createProject(payload: { title: string, type: CampaignType }) {
  try {
    const projectId = await projectsStore.createProject(payload);
    
    const payloadLineItem = {
      campaignId: projectId,
      quantity: 1,
      productId: props.productId,
    }
    
    const lineItemId = await useLineItemsStore().createLineItem(payloadLineItem);

    return lineItemId;
  } catch(err) {
    ShowAlert([{
      label: 'Network request error',
      message: String(err),
    }]);
    return undefined;
  }
}
</script>

<template>
  <form 
    class="form-setup"
    @submit.prevent
  >
    <div class="form-setup__dropdown-wrap">
      <BaseLabel
        label="What type of Project will this be?"
      />
      <BaseDropdown
        class="form-setup__input"
        name="projectType"
        placeholder="Select a Project Type"
        :options="projectTypesList"
        :error="projectTypeField.error"
        @update:dropdown-value="updateFieldValue"
      />
    </div>
    <div class="form-setup__name-input-wrap">
      <BaseLabel
        label="What should we call your Project?"
      />
      <BaseInput
        name="title"
        class="form-setup__input"
        @pressed-enter="submitProject"
      />
    </div>
    <base-button
      tag="button"
      type="submit"
      native-type="submit"
      style-type="primary"
      :click="submitProject"
      class="form-setup__button-submit"
    >
      Get Started!
    </base-button>
  </form>
</template>

<style scoped lang="scss">
.form-setup {
  display: flex;
  justify-content: space-between;
  align-items: flex-end;
  gap: 20px;

  width: 100%;

  margin: 40px 0;

  &__dropdown-wrap {
    width: 100%;
    max-width: 510px;
  }

  &__name-input-wrap {
    width: 100%;
  }

  &__input {
    height: 44px !important;
  }

  &__button-submit {
    min-width: 200px;
    max-width: 200px;
    height: 44px;
  }
}
</style>